using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject Tutorial;

    public void OnStartButton()
    {
        MainMenu.SetActive(false);
        Tutorial.SetActive(true);
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    public void OnContinueButton()
    {
        SceneManager.LoadScene("Game");
        AudioManager.Instance.PlayMusic("Game");
    }
}
