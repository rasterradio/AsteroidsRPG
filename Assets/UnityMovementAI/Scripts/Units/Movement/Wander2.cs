﻿using UnityEngine;

namespace UnityMovementAI
{
    [RequireComponent(typeof(SteeringBasics))]
    public class Wander2 : MonoBehaviour
    {
        public float wanderRadius = 1.2f;

        public float wanderDistance = 2f;

        /// <summary>
        /// Maximum amount of random displacement a second
        /// </summary>
        public float wanderJitter = 40f;

        Vector3 wanderTarget;

        SteeringBasics steeringBasics;

        void Awake()
        {
            steeringBasics = GetComponent<SteeringBasics>();
        }

        void Start()
        {
            /* Set up the wander target. Doing this in Start() because the MovementAIRigidbody
             * sets itself up in Awake(). */
            float theta = Random.value * 2 * Mathf.PI;

            /* Create a vector to a target position on the wander circle */
            wanderTarget = new Vector3(wanderRadius * Mathf.Cos(theta), wanderRadius * Mathf.Sin(theta), 0f);
        }

        public Vector3 GetSteering()
        {
            /* Get the jitter for this time frame */
            float jitter = wanderJitter * Time.deltaTime;

            /* Add a small random vector to the target's position */
            wanderTarget += new Vector3(Random.Range(-1f, 1f) * jitter, Random.Range(-1f, 1f) * jitter, 0f);

            /* Make the wanderTarget fit on the wander circle again */
            wanderTarget.Normalize();
            wanderTarget *= wanderRadius;

            /* Move the target in front of the character */
            Vector3 targetPosition = transform.position + transform.right * wanderDistance + wanderTarget;

            //Debug.DrawLine(transform.position, targetPosition);

            return steeringBasics.Seek(targetPosition);
        }
    }
}