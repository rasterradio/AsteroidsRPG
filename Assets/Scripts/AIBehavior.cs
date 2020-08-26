using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    /*Transform target;
    public float speed = 1000f;
    public float torque = 500f;
    public float nextWayPointDistance = 3f;

    bool reachedEndOfPath;
    int currentWayPoint = 0;

    Path path;
    Seeker seeker;
    Rigidbody2D rb;

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    private void OnEnable() { target = GameObject.FindGameObjectWithTag("Player").transform; }

    void UpdatePath()
    {
        if (seeker.IsDone()) { seeker.StartPath(rb.position, target.position, OnPathComplete); }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    private void Update()
    {
        if (path == null) return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else { reachedEndOfPath = false; }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if (distance < nextWayPointDistance) { currentWayPoint++; }
    }

    private void FixedUpdate()
    {
        //Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        //Vector2 force = direction * speed * Time.deltaTime;
        Vector3 force2 = transform.up * speed * Time.deltaTime;
        float turn = torque * Time.deltaTime;
        float zTorque = transform.forward.z * -turn;
        //rb.AddTorque(zTorque);
        rb.AddForce(force2);
    }*/
}
