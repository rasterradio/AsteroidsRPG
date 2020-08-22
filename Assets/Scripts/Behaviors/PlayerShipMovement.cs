using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShipMovement : MonoBehaviour
{
    public float fuel = 300f;
    private float fuelUsage;

    public float thrust = 1000f;
    public float torque = 500f;
    public float maxSpeed = 20f;

    [SerializeField]
    float acceleration = 10f, turnSpeed;
    Vector3 currVelocity;
    float brake = 0f;

    Rigidbody2D rb;
    float thrustInput;
    float turnInput;

    [SerializeField]
    private bool hasFuel = true;

    private GameObject HUD;

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

    private void Start()
    {
        HUD = GameObject.Find("HUDCanvas");
        if (HUD == null) { Debug.LogError("HUDCanvas could not be found!"); }
    }

    void Awake() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        turnInput = ShipInput.GetTurnAxis();
        thrustInput = ShipInput.GetForwardThrust();
        //isBraking = ShipInput.IsBraking() ? -1 : 1;
        //if (ShipInput.IsBraking()) { thrust = 10f; }
        //else { thrust = 1000f; }
        //brake = Input.GetKey("space") ? rigidbody.mass * 0.1 : 0.0;
        //if (ShipInput.IsBraking()) { rb.mass * 0.1; }
        //brake = ShipInput.IsBraking() ? rb.mass * 0.1f : 0.0f;
        //Debug.Log(brake);
        HUD.GetComponent<HUDController>().UpdateFuelText(fuel);
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
        fuelUsage = thrustInput * Time.deltaTime * 0.4f;
        fuel -= fuelUsage;
        if (fuel <= 0) { hasFuel = false; }
    }

    void Turn()
    {
        // Determine the torque based on the input, force and time between frames.
        float turn = turnInput * torque * Time.deltaTime;
        float zTorque = transform.forward.z * -turn;
        rb.AddTorque(zTorque);
    }

    void ClampSpeed() { rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed); }

    private void Accelerate()
    {
        currVelocity += transform.up * acceleration * Time.deltaTime;
        currVelocity = Vector3.ClampMagnitude(currVelocity, maxSpeed);
    }

    void Turn(bool right)
    {
        if (right) { transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime * -1); }
        else { transform.Rotate(Vector3.forward * turnSpeed * Time.deltaTime * -1); }
    }
}
