using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Ui : MonoBehaviour
{
    public static Ui Inst;
    public GameObject WinScreen; // Panel de felicitaciones
    public GameObject PauseScreen;

    public TextMeshProUGUI TimeCounterGameplay;
    public TextMeshProUGUI TimeCounterWin;

    private float TimeSeconds;
    private int TimeMinutes;
    private bool Win;
    public bool Pause;

    public Button RetryButton;
    public Button MainMenuButton;
    public Button MainMenuButtonPause;

    public Button ContinueButton;


    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;

            RetryButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            ContinueButton.onClick.AddListener(() =>
            {
                Pause = false;
                PauseScreen.SetActive(false);
                TimeCounterGameplay.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            });

            MainMenuButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });

            MainMenuButtonPause.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(0);
            });
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
            TimeCounterWin.text = "Tiempo: " + TimeMinutes + ":" + Mathf.Ceil(TimeSeconds);
            TimeCounterGameplay.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ShowPauseScreen()
    {

        PauseScreen.SetActive(true);
        TimeCounterGameplay.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Pause = true;
    }

    private void Update()
    {


        if (!Win && !Pause)
        {
            TimeSeconds += Time.deltaTime;

            if (TimeSeconds >= 59)
            {
                TimeMinutes++;
                TimeSeconds = 0;

            }

            TimeCounterGameplay.text = "Tiempo: " + TimeMinutes + ":" + Mathf.Ceil(TimeSeconds);


        }
        
    }
}

