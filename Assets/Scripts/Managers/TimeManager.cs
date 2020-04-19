using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : GenericSingleton<TimeManager>
{
    public float InitialSeconds;

    public System.Action OnTimeOver;
    public System.Action<float> OnAddTime;

    float CurSecondsLeft;
    bool m_TimeOver;
    bool m_TimeCountDown;


    public void Initialize(float seconds)
    {
        InitialSeconds = seconds;
        CurSecondsLeft = InitialSeconds;
        m_TimeOver = false;
        m_TimeCountDown = false;
    }

    public float GetCurTime()
    {
        return CurSecondsLeft;
    }

    public void AddTime(float seconds)
    {
        CurSecondsLeft += seconds;
        OnAddTime(seconds);
    }

    public void StartTimer()
    {
        if(!m_TimeCountDown)
            m_TimeCountDown = true;
    }

    public void StopTimer()
    {
        m_TimeCountDown = false;
    }

    private void Update()
    {
        if(!m_TimeOver && m_TimeCountDown)
            CurSecondsLeft -= Time.deltaTime;
        if (CurSecondsLeft <= 0 && !m_TimeOver)
        {
            TimeOver();
        }
    }

    void TimeOver()
    {
        m_TimeOver = true;
        CurSecondsLeft = 0;
        OnTimeOver();
    }

}
