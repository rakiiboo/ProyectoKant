using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class PlayerPet : MonoBehaviour
{
    public int life = 2;

    public float speed;

    public float jumpForce;

    public GameObject lifesPanel;

    public GameObject player;

    private float distance;

    private Rigidbody2D rigidBody2D;

    public float horizontal;

    public float cooldownTime = 1f;

    private bool isGrounded;

    private bool isInCooldown;

    private Animator animator;

    private Vector2 initialPosition;

    public Vector2 respawnPoint;

    private GameObject destinyWarp;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        initialPosition = transform.position;
        respawnPoint = initialPosition;
    }

    void Update()
    {
        Vector2 direction = player.transform.position - transform.position;
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (Time.timeScale > 0 && distance >= 2)
        {
            horizontal = Input.GetAxis("Horizontal") * speed;
            if (horizontal < 0.0f)
            {
                transform.localScale = new Vector2(-0.7f, 0.7f);
            }
            else if (horizontal > 0.0f)
            {
                transform.localScale = new Vector2(0.7f, 0.7f);
            }
            animator.SetBool("isRunning", horizontal != 0.0f);

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                StartCoroutine(TimeDelayJump());              
            }

            if (!isGrounded)
            {
                animator.SetBool("isJumping", true);
            }
            else
            {
                animator.SetBool("isJumping", false);
            }         

            if (Input.GetKeyDown(KeyCode.S) && destinyWarp)
            {
                transform.position = destinyWarp.transform.position;
            }        

            DeathOnFall();
        } 
        else
        {
            
        }
    }

    public void Death()
    {
        transform.position = initialPosition;
        respawnPoint = initialPosition;
        if (life < 0)
        {
            StartCoroutine(retrasoEscena("Lose"));
            //life = 2;
            //for(int i = 0; i < lifesPanel.transform.childCount; i++)
            //{
            //    lifesPanel.transform.GetChild(i).gameObject.SetActive(true);
            //}
        }
    }

    public void Hit(float knockback, GameObject enemy)
    {
        if (!isInCooldown)
        {
            StartCoroutine(cooldown());
            if (life >= 0)
            {
                lifesPanel.transform.GetChild(life).gameObject.SetActive(false);
                life -= 1;
                if (enemy)
                {
                    Vector2 difference = (transform.position - enemy.transform.position);
                    float knockbackDirection = difference.x >= 0 ? 1 : -1;
                    rigidBody2D.velocity = new Vector2(knockbackDirection * knockback, knockback);
                }
            }
            else
            {
                Death();
            }
        }
    }

    IEnumerator TimeDelayJump()
    {
        yield return new WaitForSeconds(0.1f);

        Jump();
    }

    IEnumerator cooldown()
    {
        isInCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        isInCooldown = false;
    }

    IEnumerator retrasoEscena(string sceneName)
    {
        yield return new WaitForSecondsRealtime(0.5f);
        SceneManager.LoadScene(sceneName);
    }

    private void Jump()
    {      
        rigidBody2D.AddForce(Vector2.up * jumpForce);
    }

    private void DeathOnFall()
    {
        if (transform.position.y < -25f)
        {
            transform.position = respawnPoint;
            Hit(0, null);
        }
    }

    private void FixedUpdate()
    {
        if (!isInCooldown)
        {
            rigidBody2D.velocity = new Vector2(horizontal, rigidBody2D.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "Tilemap")
        {
            isGrounded = true;
        }
        if (collider.name == "warpA" || collider.name == "warpB")
        {
            GameObject warp = collider.transform.parent.gameObject;
            if (collider.name == "warpA")
            {
                destinyWarp = warp.transform.Find("warpB").gameObject;
            }
            else
            {
                destinyWarp = warp.transform.Find("warpA").gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.name == "Tilemap")
        {
            isGrounded = false;
        }
        if (collider.name == "warpA" || collider.name == "warpB")
        {
            destinyWarp = null;
        }
    }
}
