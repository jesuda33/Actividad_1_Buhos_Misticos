using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuSript : MonoBehaviour
{
    // Menu de inicio y bienvenida
    public Button PlayButton;
    public Button ExitButton;


    private void Awake()
    {
        PlayButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(1);
        });

        ExitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }



}
