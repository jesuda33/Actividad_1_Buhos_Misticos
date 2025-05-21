using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class IniciarJuego : MonoBehaviour
{
    
    // Menu de inicio y bienvenida
    public Button PlayButton;
    public Button ExitButton;

    void Awake() //inicio y salir del juego
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
