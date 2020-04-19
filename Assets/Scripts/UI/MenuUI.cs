using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene("Game");
        AudioManager.Instance.PlayMusic("Game");
    }

    public void OnExitButton()
    {
        Application.Quit();
    }
}
