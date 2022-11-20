using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public AudioSource audioSource1;
    public AudioSource audioSource2;

    public void CambiarEscenaClick(string sceneName)
    {
        Debug.Log("Cambiando de Escena: " + sceneName);
        StartCoroutine(retrasoEscena(sceneName));
        audioSource2.Play();
    }

    public void Salirjuego()
    {
        Debug.Log("Saliendo del Juego");
        Application.Quit();
        audioSource2.Play();
    }

    IEnumerator retrasoEscena(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(sceneName);
    }
}
