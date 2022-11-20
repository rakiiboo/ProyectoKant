using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : Enemy
{

    public float speed = 10f;

    public float direction = 1f;

    public float directionTimeChange = 5f;

    private Rigidbody2D rigidBody2D;

    private GameObject wallL;

    private GameObject wallR;

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        wallL = transform.parent.Find("WallR").gameObject;
        wallR = transform.parent.Find("WallL").gameObject;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rigidBody2D.velocity = new Vector2(direction * speed, rigidBody2D.velocity.y);
    }

    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        if (collider.gameObject == wallL || collider.gameObject == wallR)
        {
            Turn();
        }
    }

    private void Turn()
    {
        direction = direction * -1;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
}
