using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    public float RewardSeconds;
    bool m_HasPassed = false;

    public static System.Action OnPassed;

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Ball" && !m_HasPassed)
        {
            m_HasPassed = true;
            TimeManager.Instance.AddTime(RewardSeconds);
            OnPassed();
        }
    }
}
