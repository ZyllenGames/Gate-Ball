using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                GameObject managerObject = GameObject.Find("Manager");
                if (managerObject == null)
                {
                    managerObject = new GameObject("Manager");
                    DontDestroyOnLoad(managerObject);
                }
                m_Instance = managerObject.AddComponent<T>();
            }
            return m_Instance;
        }
    }
}
