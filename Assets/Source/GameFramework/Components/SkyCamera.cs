using UnityEngine;

public class SkyCamera : MonoBehaviour
{
    public Transform follow;

    [SerializeField]
    private float m_followSpeedFactor = 1.0f;
    private Vector3 m_velocity;

    public Camera cameraComponent { get; set; }


    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
    }


    private void LateUpdate()
    {
        if (follow == null)
            return;

        Vector3 position = transform.position;
        position = Vector3.SmoothDamp(position, follow.position, ref m_velocity, m_followSpeedFactor * Time.deltaTime);
        transform.position = position;
    }


    public Vector3 GetVelocity()
    {
        return m_velocity;
    }
}
