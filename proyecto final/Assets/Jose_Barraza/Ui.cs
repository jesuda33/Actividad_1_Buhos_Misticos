using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ui : MonoBehaviour
{
    public static Ui Inst;

    public GameObject WinScreen;
    public GameObject LoseScreen;
    public GameObject PauseScreen;

    public TextMeshProUGUI TimeCounterGameplay;
    public TextMeshProUGUI TimeCounterWin;
    public TextMeshProUGUI TimeCounterLose;

    public int totalTimeInSeconds = 180; // Tiempo límite (3 minutos)
    private float remainingTime;
    private bool timerRunning = true;

    private bool Win;
    public bool Pause;

    public int score = 0;
    public TextMeshProUGUI scoreText;

    public Button RetryButton;
    public Button MainMenuButton;
    public Button MainMenuButtonPause;
    public Button ContinueButton;

    // Nuevos botones para la pantalla de "Lose"
    public Button RetryButtonLose;
    public Button MainMenuButtonLose;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;

            // Botones de pantalla "Win" y "Pause"
            RetryButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            ContinueButton.onClick.AddListener(() =>
            {
                Pause = false;
                PauseScreen.SetActive(false);
                TimeCounterGameplay.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                timerRunning = true;
                Time.timeScale = 1f;
            });

            MainMenuButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            });

            MainMenuButtonPause.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            });

            // Botones de pantalla "Lose"
            RetryButtonLose.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            MainMenuButtonLose.onClick.AddListener(() =>
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(0);
            });

            remainingTime = totalTimeInSeconds;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowScreen()
    {
        if (WinScreen != null)
        {
            WinScreen.SetActive(true);
            Win = true;
            timerRunning = false;

            float usedTime = totalTimeInSeconds - remainingTime;
            TimeCounterWin.text = "Tiempo completado: " + FormatTime(usedTime);

            TimeCounterGameplay.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    public void ShowLoseScreen()
    {
        if (LoseScreen != null)
        {
            LoseScreen.SetActive(true);
            Pause = true;
            timerRunning = false;
            TimeCounterLose.text = " " + FormatTime(0);
            TimeCounterGameplay.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    public void AddPoints(int points)
    {
        score += points;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Puntos: " + score.ToString();
        }
    }

    public void ShowPauseScreen()
    {
        PauseScreen.SetActive(true);
        TimeCounterGameplay.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Pause = true;
        timerRunning = false;
        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (!Win && !Pause && timerRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                if (remainingTime < 0) remainingTime = 0;
                TimeCounterGameplay.text = " " + FormatTime(remainingTime);
            }
            else
            {
                ShowLoseScreen();
            }
        }
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
