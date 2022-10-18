using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    public AudioClip SonidoGolpe;

    private AudioSource audioSource;

    public void CambiarEscenaClick(string sceneName)
    {
        Debug.Log("Cambiando de Escena: " + sceneName);
        StartCoroutine(retrasoEscena(sceneName));
        GetComponent<AudioSource>().Play();
    }

    public void Salirjuego()
    {
        Debug.Log("Saliendo del Juego");
        Application.Quit();
        GetComponent<AudioSource>().Play();
    }

    IEnumerator retrasoEscena(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
