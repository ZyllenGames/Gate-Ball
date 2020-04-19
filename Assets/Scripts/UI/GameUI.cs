using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Text TimeText;
    public GameObject AddTimeObj;
    public GameObject FadeIn;
    public GameObject FadeOut;
    public GameObject LevelWinPanel;
    public GameObject LevelLosePanel;

    public LevelManager LevelManager;

    private void Awake()
    {
        TimeManager.Instance.OnTimeOver += LevelLose;
        TimeManager.Instance.OnAddTime += AddTime;
        LevelManager.OnLevelWin += LevelWin;
    }

    private void Start()
    {
        TimeText.text = "Time Left: " + string.Format("{0:F2}", TimeManager.Instance.GetCurTime()) + "s";
        AddTimeObj.SetActive(false);
    }

    private void Update()
    {
        TimeText.text = "Time Left: " + string.Format("{0:F2}", TimeManager.Instance.GetCurTime()) + "s";
    }

    void AddTime(float seconds)
    {
        StartCoroutine(TimeTextShow(seconds));
    }

    IEnumerator TimeTextShow(float sec)
    {
        AddTimeObj.GetComponent<Text>().text = "+" + sec.ToString() + "s";
        AddTimeObj.SetActive(true);
        yield return new WaitForSeconds(1);
        AddTimeObj.SetActive(false);
    }

    void LevelWin()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        LevelWinPanel.SetActive(true);
        string winText = "Level " + (LevelManager.CurLevelID + 1).ToString() + " Completed!";
        LevelWinPanel.GetComponentInChildren<Text>().text = winText;

        TimeManager.Instance.StopTimer();
    }

    void LevelLose()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        LevelLosePanel.SetActive(true);
        string loseText = "Level " + (LevelManager.CurLevelID + 1).ToString() + " Failed...";
        LevelLosePanel.GetComponentInChildren<Text>().text = loseText;

        TimeManager.Instance.StopTimer();
    }

    public void OnContinueButton()
    {
        StartCoroutine(NextLevel());
    }

    IEnumerator NextLevel()
    {
        LevelEndFade();
        yield return new WaitForSeconds(1);
        LevelWinPanel.SetActive(false);
        LevelManager.LevelContinue();
        TimeManager.Instance.Initialize(LevelManager.CurLevelID * 10 + 10);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        LevelStartFade();
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
    }
    public void OnRestartButton()
    {
        StartCoroutine(RestartLevel());
    }

    IEnumerator RestartLevel()
    {
        LevelEndFade();
        yield return new WaitForSeconds(1);
        LevelLosePanel.SetActive(false);
        LevelManager.LevelRestart();
        TimeManager.Instance.Initialize(LevelManager.CurLevelID * 10 + 10);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        LevelStartFade();
        yield return new WaitForSeconds(1);
        FadeIn.SetActive(false);
    }

    void LevelStartFade()
    {
        FadeIn.SetActive(true);
        FadeOut.SetActive(false);
    }
    void LevelEndFade()
    {
        FadeOut.SetActive(true);
        FadeIn.SetActive(false);
    }
}
