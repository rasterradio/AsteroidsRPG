using UnityEngine;
using System.Collections;

namespace UnityMovementAI
{
    /// <summary>
    /// This is a wrapper class for either a Rigidbody or Rigidbody2D, so that either can be used with the Unity Movement AI code. 
    /// </summary>
    public class MovementAIRigidbody : MonoBehaviour
    {
        /// <summary>
        /// How far the character should look below him for ground to stay grounded to
        /// </summary>
        public float groundFollowDistance = 0.1f;

        /// <summary>
        /// The sphere cast mask that determines what layers should be consider the ground
        /// </summary>
        public LayerMask groundCheckMask = Physics.DefaultRaycastLayers;

        /// <summary>
        /// The maximum slope the character can climb in degrees
        /// </summary>
        public float slopeLimit = 80f;

        CircleCollider2D col2D;

        /// <summary>
        /// The radius for the current game object (either the radius of a sphere or circle
        /// collider). If the game object does not have a sphere or circle collider this 
        /// will return -1.
        /// </summary>
        public float Radius
        {
            get
            {
                if (col2D != null)
                {
                    return Mathf.Max(rb2D.transform.localScale.x, rb2D.transform.localScale.y) * col2D.radius;
                }
                else
                {
                    return -1;
                }
            }
        }

        Rigidbody2D rb2D;

        void Awake()
        {
            SetUp();
        }

        /// <summary>
        /// Sets up the MovementAIRigidbody so it knows about its underlying collider and rigidbody.
        /// </summary>
        public void SetUp()
        {
            SetUpRigidbody();
            SetUpCollider();
        }

        void SetUpRigidbody()
        {
            rb2D = GetComponent<Rigidbody2D>();
        }

        void SetUpCollider()
        {
            CircleCollider2D col = rb2D.GetComponent<CircleCollider2D>();

            if (col != null)
            {
                col2D = col;
            }
        }

        void Start()
        {
            StartCoroutine(DebugDraw());

            /* Call fixed update for 3D grounded characters to make sure they get proper 
             * ground / movement normals before their velocity is set */
        }

        int count = 0;
        int countDebug = 0;

        IEnumerator DebugDraw()
        {
            yield return new WaitForFixedUpdate();

            Vector3 origin = ColliderPosition;
            Debug.DrawLine(origin, origin + (Velocity.normalized), Color.red, 0f, false);

            count++;
            countDebug = 0;
            StartCoroutine(DebugDraw());
        }

        /* Make the spherecast offset slightly bigger than the max allowed collider overlap. This was
         * known as Physics.minPenetrationForPenalty and had a default value of 0.05f, but has since
         * been removed and supposedly replaced by Physics.defaultContactOffset/Collider.contactOffset.
         * My tests show that as of Unity 5.3.0f4 this is not %100 true and Unity still seems to be 
         * allowing overlaps of 0.05f somewhere internally. So I'm setting my spherecast offset to be
         * slightly bigger than 0.05f */
        readonly float spherecastOffset = 0.051f;

        bool SphereCast(Vector3 dir, out RaycastHit hitInfo, float dist, int layerMask, Vector3 planeNormal = default(Vector3))
        {
            dir.Normalize();

            /* Make sure we use the collider's origin for our cast (which can be different
             * then the transform.position).
             *
             * Also if we are given a planeNormal then raise the origin a tiny amount away
             * from the plane to avoid problems when the given dir is just barely moving  
             * into the plane (this can occur due to floating point inaccuracies when the 
             * dir is calculated with cross products) */
            Vector3 origin = ColliderPosition + (planeNormal * 0.001f);

            /* Start the ray with a small offset from inside the character, so it will
             * hit any colliders that the character is already touching. */
            origin += -spherecastOffset * dir;

            float maxDist = (spherecastOffset + dist);

            if (Physics.SphereCast(origin, Radius, dir, out hitInfo, maxDist, layerMask))
            {
                /* Remove the small offset from the distance before returning*/
                hitInfo.distance -= spherecastOffset;
                return true;
            }
            else
            {
                return false;
            }
        }

        bool IsWall(Vector3 surfNormal)
        {
            /* If the normal of the surface is greater then our slope limit then its a wall */
            return Vector3.Angle(Vector3.up, surfNormal) > slopeLimit;
        }

        bool IsMovingInto(Vector3 dir, Vector3 normal)
        {
            return Vector3.Angle(dir, normal) > 90f;
        }

        /// <summary>
        /// The position that should be used for most movement AI code. For 2D chars the position will 
        /// be on the X/Y plane. For 3D grounded characters the position is on the X/Z plane. For 3D
        /// flying characters the position is in full 3D (X/Y/Z).
        /// </summary>
        public Vector3 Position
        {
            get
            {
                return rb2D.position;
            }
        }

        /// <summary>
        /// Gets the position of the collider (which can be offset from the transform position).
        /// </summary>
        public Vector3 ColliderPosition
        {
            get
            {
                return Transform.TransformPoint(col2D.offset);
            }
        }

        /// <summary>
        /// The velocity that should be used for movement AI code. For 2D chars this velocity will be on 
        /// the X/Y plane. For 3D grounded characters this velocity will be on the X/Z plane but will be
        /// applied on whatever plane the character is currently moving on. For 3D flying characters the
        /// velocity will be in full 3D (X/Y/Z).
        /// </summary>
        public Vector3 Velocity
        {
            get
            {
                return rb2D.velocity;
            }

            set
            {

                rb2D.velocity = value;
            }
        }

        /// <summary>
        /// The actual velocity of the underlying unity rigidbody.
        /// </summary>
        public Vector3 RealVelocity
        {
            get
            {
                return rb2D.velocity;
            }
            set
            {
                rb2D.velocity = value;
            }
        }

        /// <summary>
        /// Creates a vector that maintains x/z direction but lies on the plane.
        /// </summary>
        Vector3 DirOnPlane(Vector3 vector, Vector3 planeNormal)
        {
            Vector3 newVel = vector;
            newVel.y = (-planeNormal.x * vector.x - planeNormal.z * vector.z) / planeNormal.y;
            return newVel.normalized;
        }

        public Transform Transform
        {
            get
            {
                return rb2D.transform;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                Quaternion r = Quaternion.identity;
                r.eulerAngles = new Vector3(0, 0, rb2D.rotation);
                return r;
            }

            set
            {
                rb2D.MoveRotation(value.eulerAngles.z);
            }
        }

        /// <summary>
        /// The angularVelocity for the rigidbody. If its a 3D rigidbody underneath then the angularVelocity is for the y axis only (setting the angular velocity will clear out the x/z angular velocities).
        /// </summary>
        public float AngularVelocity
        {
            get
            {
                return rb2D.angularVelocity;
            }

            set
            {
                rb2D.angularVelocity = value;
            }
        }

        /// <summary>
        /// Rotates the rigidbody to angle (given in degrees)
        /// </summary>
        /// <param name="angle"></param>
        public void MoveRotation(float angle)
        {
            rb2D.MoveRotation(angle);
        }

        public float RotationInRadians
        {
            get
            {
                return rb2D.rotation * Mathf.Deg2Rad;
            }
        }

        public Vector3 RotationAsVector
        {
            get
            {
                return SteeringBasics.OrientationToVector(RotationInRadians);
            }
        }

        /// <summary>
        /// Converts the vector based what kind of character the rigidbody is on. 
        /// If it is a 2D character then the Z component will be zeroed out. If it
        /// is a grounded 3D character then the Y component will be zeroed out. 
        /// And if it is flying 3D character no changes will be made to the vector.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 ConvertVector(Vector3 v)
        {
            /* If the character is a 2D character then ignore the z component */
            v.z = 0;
            /* Else if the charater is a 3D character who can't fly then ignore the y component */

            return v;
        }

        /* This function is here to ensure we have a rigidbody (2D or 3D). */
#if UNITY_EDITOR
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public void Reset()
        {
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();

            if (rb2D == null)
            {
                gameObject.AddComponent<Rigidbody2D>();
            }
        }
#endif
    }
}