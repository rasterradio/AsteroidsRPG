using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 offset;

    [SerializeField]
    private float cameraTimeToMove = 0.03f, m_FieldOfView = 70f;

    private readonly float maxFOV = 150f, minFOV = 20f;

    public void AttachToPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }

    void Update()
    {
        if (player != null)
        {
            Vector3 velocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, cameraTimeToMove * Time.deltaTime);
            //velocity gets set to camera speed

            Camera.main.fieldOfView = m_FieldOfView;

            if (Input.GetAxis("Mouse ScrollWheel") > 0 && m_FieldOfView > minFOV) { m_FieldOfView -= 5f; }
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && m_FieldOfView < maxFOV) { m_FieldOfView += 5f; }
        }
        //else { Debug.LogWarning("Could not find player to attach camera to."); }
    }
}