using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int level = 0;

    public GameObject winSpot;

    private BoxCollider2D winCollider;

    public Player player;

    private BoxCollider2D playerCollider;

    public GameObject[] checkPoints;

    public AudioClip song;

    private AudioSource audioSource;

    public bool isFinal;

    public GameObject character1;

    public GameObject character2;

    public GameObject menuPausa;

    void Start()
    {
        winCollider = winSpot.GetComponent<BoxCollider2D>();
        playerCollider = player.GetComponent<BoxCollider2D>();
    }


    void Update()
    {

        Physics2D.IgnoreCollision(character1.GetComponent<Collider2D>(), character2.GetComponent<Collider2D>());

        if (winCollider.IsTouching(playerCollider))
        {
            if (isFinal)
            {
                SceneManager.LoadScene("Win");
            }
            else
            {
                SceneManager.LoadScene("Level" + (level + 1));
            }
        }

        foreach (var checkpoint in checkPoints)
        {
            BoxCollider2D checkPointCollider = checkpoint.GetComponent<BoxCollider2D>();
            if (checkPointCollider.IsTouching(playerCollider))
            {
                player.GetComponent<Player>().respawnPoint = player.transform.position;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if(Time.timeScale > 0)
        {
            Time.timeScale = 0;
            menuPausa.SetActive(true);
            GetComponent<AudioSource>().Pause();
        } else
        {
            Time.timeScale = 1;
            menuPausa.SetActive(false);
            GetComponent<AudioSource>().Play();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

}
