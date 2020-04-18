using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initializer : MonoBehaviour
{
    public float InitialSeconds;

    private void Awake()
    {
        TimeManager.Instance.Initialize(InitialSeconds);
    }
}
