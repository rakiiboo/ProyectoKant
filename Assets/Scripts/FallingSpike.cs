using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpike : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    public float distance;
    bool isFalling = false;
    public float gravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        Physics2D.queriesStartInColliders = false;
        if(isFalling == false)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, distance);

            Debug.DrawRay(transform.position, Vector2.down * distance, Color.red);           

            if(hit.transform != null)
            {
                if(hit.transform.tag == "Player")
                {
                    Debug.Log("Jugador detectado");
                    rb.gravityScale = gravity;
                    isFalling = true;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {  
        if (other.tag == "Player")
        {
            Debug.Log("En Colisión con el jugador");
            Destroy(gameObject, 1f);
        }
        else
        {
            rb.gravityScale = 0;
        }
    }

}
