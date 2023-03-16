using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIAutoscroll : MonoBehaviour
{
    [SerializeField]
    private ScrollRect m_scrollRect = null;
    [SerializeField]
    private float m_duration = 30.0f;


    public void Begin()
    {
        StartCoroutine(Co_Scroller());
    }


    public void Stop()
    {
        StopAllCoroutines();
        m_scrollRect.verticalNormalizedPosition = 1.0f;
    }


    private IEnumerator Co_Scroller()
    {
        var t = 0.0f;
        while (true)
        {
            t += Time.deltaTime / m_duration;
            m_scrollRect.verticalNormalizedPosition = 1.0f - Mathf.PingPong(t, 1.0f);
            yield return null;
        }
    }
}
