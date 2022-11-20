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

    public PlayerPet playerPet;

    private BoxCollider2D playerCollider;

    private BoxCollider2D playerPetCollider;

    public GameObject[] checkPoints;

    public GameObject[] getLifes;

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
        playerPetCollider = playerPet.GetComponent<BoxCollider2D>();
    }


    void Update()
    {

        Physics2D.IgnoreCollision(character1.GetComponent<Collider2D>(), character2.GetComponent<Collider2D>());

        if (winCollider.IsTouching(playerCollider))
        {
            if (isFinal)
            {
                SceneManager.LoadScene("Cutscene2");
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
            else if (checkPointCollider.IsTouching(playerPetCollider))
            {
                playerPet.GetComponent<PlayerPet>().respawnPoint = playerPet.transform.position;
            }

        }

        foreach (var getlife in getLifes)
        {
            BoxCollider2D getLifesCollider = getlife.GetComponent<BoxCollider2D>();

            if (getLifesCollider.IsTouching(playerCollider))
            {
                //Debug.Log("El jugador está tocando la vida");
                if (player.life < 2)
                {
                    player.life += 1;
                    player.lifesPanel.transform.GetChild(player.life).gameObject.SetActive(true);
                    getlife.SetActive(false);
                }
                else if (playerPet.life < 2)
                {
                    playerPet.life += 1;
                    playerPet.lifesPanel.transform.GetChild(playerPet.life).gameObject.SetActive(true);
                    getlife.SetActive(false);
                }
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
