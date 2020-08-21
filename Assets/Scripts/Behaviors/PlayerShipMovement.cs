using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShipMovement : MonoBehaviour
{
    public float fuel = 300f;
    private float fuelUsage;

    public float thrust = 1000f;
    public float torque = 500f;
    public float maxSpeed = 20f;
    float brake = 0f;

    Rigidbody rb;
    float thrustInput;
    float turnInput;

    [SerializeField]
    private bool hasFuel = true;

    void ResetInput()
    {
        thrustInput = 0f;
        turnInput = 0f;
    }

    void ResetSpeed()
    {
        thrust = 1000f;
        torque = 500f;
    }

    void Awake() { rb = GetComponent<Rigidbody>(); }

    void Update()
    {
        turnInput = ShipInput.GetTurnAxis();
        thrustInput = ShipInput.GetForwardThrust();
        //isBraking = ShipInput.IsBraking() ? -1 : 1;
        //if (ShipInput.IsBraking()) { thrust = 10f; }
        //else { thrust = 1000f; }
        //brake = Input.GetKey("space") ? rigidbody.mass * 0.1 : 0.0;
        //if (ShipInput.IsBraking()) { rb.mass * 0.1; }
        brake = ShipInput.IsBraking() ? rb.mass * 0.1f : 0.0f;
        Debug.Log(brake);
    }

    void FixedUpdate() { Move(); Turn(); ClampSpeed(); }

    void Move()
    {
        if (hasFuel)
        {
            // Create a vector in the direction the ship is facing.
            // Magnitude based on the input, speed and the time between frames.
            Vector3 thrustForce = thrustInput * thrust * Time.deltaTime * transform.up;
            rb.AddForce(thrustForce);
            FuelBurn();
        }
    }

    void FuelBurn()
    {
        fuelUsage = thrustInput * Time.deltaTime * 0.3f;
        fuel -= fuelUsage;
        if (fuel <= 0) { hasFuel = false; }
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        float turn = turnInput * torque * Time.deltaTime;
        Vector3 zTorque = transform.forward * -turn;
        rb.AddTorque(zTorque);
    }

    void ClampSpeed()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
