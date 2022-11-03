using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxFondo : MonoBehaviour
{
    [SerializeField] private Vector2 velocidadMovimiento;

    private Vector2 offset;

    private Material material;
    private Rigidbody2D jugadorRB;

    private void Awake()
    {
        material = GetComponent<SprinteRenderer>().material;
        jugadorRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
      offset = (jugadorRB.velocity.x * 0.1f) * velocidadMovimiento * Time.delaTime;
      material.mainTextureOffset += offset;  
    }
}
