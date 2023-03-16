using UnityEngine;

public class SimpleDrawGizmo : MonoBehaviour
{
    public string iconName = string.Empty;
    public bool allowScaling = false;
    private Camera m_cam;


    private void OnEnable()
    {
        m_cam = GetComponent<Camera>();
        m_cam.enabled = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, iconName, allowScaling);
    }
}
