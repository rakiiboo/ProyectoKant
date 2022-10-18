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

    void Start()
    {
        winCollider = winSpot.GetComponent<BoxCollider2D>();
        playerCollider = player.GetComponent<BoxCollider2D>();
    }


    void Update()
    {
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
            GetComponent<AudioSource>().Pause();
        } else
        {
            Time.timeScale = 1;
            GetComponent<AudioSource>().Play();
        }
    }
}
