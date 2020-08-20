using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    private Vector3 offset;

    [SerializeField]
    private float cameraTimeToMove = 0.03f;
    [SerializeField]
    private float m_FieldOfView = 70f;

    private readonly float maxFOV = 150f, minFOV = 20f;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, cameraTimeToMove);
        //velocity gets set to camera speed

        Camera.main.fieldOfView = m_FieldOfView;

        if (Input.GetAxis("Mouse ScrollWheel") < 0 && m_FieldOfView > minFOV) { m_FieldOfView -= 5f; }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && m_FieldOfView < maxFOV) { m_FieldOfView += 5f; }
    }
}