using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShipMovement : MonoBehaviour
{
    public float thrust = 1000f;
    public float torque = 500f;
    public float maxSpeed = 20f;

    Rigidbody rb;
    float thrustInput;
    float turnInput;

    void Reset()
    {
        thrustInput = 0f;
        turnInput = 0f;
    }

    void Awake() { rb = GetComponent<Rigidbody>(); }

    void Update()
    {
        turnInput = ShipInput.GetTurnAxis();
        thrustInput = ShipInput.GetForwardThrust();
    }

    void FixedUpdate() { Move(); Turn(); }

    void Move()
    {
        // Create a vector in the direction the ship is facing.
        // Magnitude based on the input, speed and the time between frames.
        Vector3 thrustForce = thrustInput * thrust * Time.deltaTime * transform.up;
        rb.AddForce(thrustForce);
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        float turn = turnInput * torque * Time.deltaTime;
        Vector3 zTorque = transform.forward * -turn;
        rb.AddTorque(zTorque);
    }
}
