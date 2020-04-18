using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text TimeText;

    private void Start()
    {
        TimeText.text = "Time Left: " + string.Format("{0:F2}", TimeManager.Instance.GetCurTime()) + "s";
    }

    private void Update()
    {
        TimeText.text = "Time Left: " + string.Format("{0:F2}", TimeManager.Instance.GetCurTime()) + "s";
    }
}
