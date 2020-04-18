using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : GenericSingleton<TimeManager>
{
    public float InitialSeconds;
    float CurSecondsLeft;

    bool m_TimeOver;
    public void Initialize(float seconds)
    {
        InitialSeconds = seconds;
    }
    private void Start()
    {
        CurSecondsLeft = InitialSeconds;
        m_TimeOver = false;
    }

    public float GetCurTime()
    {
        return CurSecondsLeft;
    }

    public void AddTime(float seconds)
    {
        CurSecondsLeft += seconds;
    }

    private void Update()
    {
        if(!m_TimeOver)
            CurSecondsLeft -= Time.deltaTime;
        if (CurSecondsLeft <= 0 && !m_TimeOver)
            TimeOver();
    }

    void TimeOver()
    {
        m_TimeOver = true;
        CurSecondsLeft = 0;
        print("Time Over!");
    }

}
