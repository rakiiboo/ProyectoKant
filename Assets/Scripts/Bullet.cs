using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 1.0f;

    public bool friendBullet = true;

    private Rigidbody2D rigidBody2D;

    private Vector2 direction;

    public AudioClip SonidoDisparo;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().Play();
        rigidBody2D = GetComponent<Rigidbody2D>();
        StartCoroutine(BulletTime());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidBody2D.velocity = direction * speed;
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Enemy enemy = collider.GetComponent<Enemy>();
        if (enemy != null && friendBullet)
        {
            enemy.Hit();
            DestroyBullet();
        }
    }

    IEnumerator BulletTime()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
