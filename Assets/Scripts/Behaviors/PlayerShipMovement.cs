using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerShipMovement : MonoBehaviour
{
    [SerializeField] float thrustSpeed = 15f, torqueForce = 45f, maxSpeed = 13f;
    [SerializeField] float brakingForce = 10f, maxBrakingTorque = 60f, maxBrakingDrag = 20f;

    Rigidbody2D rb;
    bool brakingInput;
    float turnInput, thrustInput, currBrakeTorque, currBrakeDrag;

    [SerializeField] bool hasFuel = true;
    [SerializeField] float fuel = 300f;
    float fuelUsage;

    GameObject HUD;

    void ResetInput()
    {
        thrustInput = 0f;
        brakingInput = false;
        turnInput = 0f;
    }

    void ResetSpeed()
    {
        thrustSpeed = 15f;
        torqueForce = 45f;
    }

    private void Start()
    {
        HUD = GameObject.Find("HUDCanvas");
        if (HUD == null) { Debug.LogError("HUDCanvas could not be found!"); }
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
        HUD.GetComponent<HUDController>().UpdateFuelText(fuel);
    }

    private void FixedUpdate()
    {
        Move(); Turn(); ClampSpeed(); Brake();
    }

    void Move()
    {
        if (hasFuel)
        {
            Vector2 thrustForce = thrustInput * thrustSpeed * transform.up;
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
        float turn = turnInput * torqueForce;
        float zTorque = transform.forward.z * -turn;
        rb.AddTorque(zTorque);
    }

    void ClampSpeed() { rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed); }

    void Brake()
    {
        if (brakingInput)
        {
            if (currBrakeTorque < maxBrakingTorque) { currBrakeTorque++; }
            if (currBrakeDrag < maxBrakingDrag) { currBrakeDrag += 8; }
        }
        else
        {
            currBrakeTorque--;
            currBrakeDrag -= 3;
        }
        rb.drag = currBrakeDrag;
        //rb.AddTorque(brakingForce);
        //rb.AddForce(transform.up * -speedForce / 8f);
    }

    Vector2 ForwardVelocity()
    {
        return transform.up * Vector2.Dot(GetComponent<Rigidbody2D>().velocity, transform.up);
    }
}
