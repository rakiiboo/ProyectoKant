using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool indestructible = false;

    public float life = 1;

    public float knockback = 10f;

    public AudioClip SonidoGolpe;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit()
    {
        life -= 1;
        GetComponent<AudioSource>().Play();
        if (!indestructible && life <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        Player player = collider.GetComponent<Player>();
        PlayerPet playerPet = collider.GetComponent<PlayerPet>();
        FollowPlayer followplayer = collider.GetComponent<FollowPlayer>();

        if (player != null)
        {
            GetComponent<AudioSource>().Play();
            player.Hit(knockback, gameObject);
        }
        else if (playerPet != null)
        {
            GetComponent<AudioSource>().Play();
            playerPet.Hit(knockback, gameObject);
        }
        else if (followplayer != null)
        {
            GetComponent<AudioSource>().Play();
            followplayer.Hit(knockback, gameObject);
        }
    }

}
