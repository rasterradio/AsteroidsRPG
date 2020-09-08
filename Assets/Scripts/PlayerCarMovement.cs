using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerCarMovement : MonoBehaviour
{
    public float thrustSpeed = 15f, torqueForce = -200f, maxSpeed = 13f;
    float driftFactorSticky = 0.9f, driftFactorSlippy = 1, maxStickyVelocity = 2.5f;

    Rigidbody2D rb;
    bool brakingInput;
    float turnInput, thrustInput;
    void ResetInput()
    {
        thrustInput = 0f;
        brakingInput = false;
        turnInput = 0f;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        ResetInput();
    }

    private void Update()
    {
        turnInput = ShipInput.GetTurnAxis();
        thrustInput = ShipInput.GetForwardThrust();
        brakingInput = ShipInput.IsBraking();
    }

    private void FixedUpdate()
    {
        Move(); Turn(); ClampSpeed();
        if (brakingInput) { Brake(); }
    }

    void Move()
    {
        float driftFactor = driftFactorSticky; //this block only affected by drifting?
        if (RightVelocity().magnitude > maxStickyVelocity)
        {
            driftFactor = driftFactorSlippy;
        }
        rb.velocity = ForwardVelocity() + RightVelocity() * driftFactor;

        // Create a vector in the direction the ship is facing.
        // Magnitude based on the input, speed and the time between frames.
        Vector2 thrustForce = thrustInput * thrustSpeed * transform.up;
        rb.AddForce(thrustForce);
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        //float tf = Mathf.Lerp(0, torqueForce, rb.velocity.magnitude / 2);
        //rb.angularVelocity = Input.GetAxis("Horizontal") * tf;

        float turn = turnInput * torqueForce;
        float zTorque = transform.forward.z * -turn;
        rb.AddTorque(zTorque);
    }

    void ClampSpeed() { rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed); }

    void Brake()
    {
        //rb.AddTorque?
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }

    Vector2 RightVelocity()
    {
        return transform.right * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.right);
    }
}
