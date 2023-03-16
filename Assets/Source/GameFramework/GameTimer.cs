using UnityEngine;

public class GameTimer : ScriptableObject
{
    private bool m_isStart;
    private bool m_isPaused;
    private float m_time;


    private void OnEnable()
    {
        m_isStart = false;
        m_isPaused = false;
        m_time = 0.0f;
    }


    private void OnDisable()
    {
        m_isStart = false;
        m_isPaused = false;
        m_time = 0.0f;
    }


    public void Update()
    {
        if (m_isStart)
        {
            if (m_isPaused)
                return;
            m_time += Time.deltaTime;
        }
    }


    public void Start()
    {
        m_isStart = true;
        m_isPaused = false;
        m_time = 0.0f;
    }


    public void Stop()
    {
        m_isStart = false;
        m_isPaused = false;
        m_time = 0.0f;
    }


    public void Pause()
    {
        // If the timer has never started, we can't possibly pause it
        if (!m_isStart)
            return;
        m_isPaused = true;
    }


    public void UnPause()
    {
        // If the timer has never started, we can't possibly unpaused it
        if (!m_isStart)
            return;
        m_isPaused = false;
    }


    public float GetTime()
    {
        return m_time;
    }
}
