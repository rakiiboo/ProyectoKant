using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class NPC : MonoBehaviour
{
    public GameObject pressPanel;
    public GameObject dialoguePanel;
    public bool playerIsClose;

    void Update()
    {     
        if (Input.GetKeyDown(KeyCode.F) && playerIsClose)
        {
            dialoguePanel.SetActive(true);
            trigger.StartDialogue();
        }
    }

    public DialogueTrigger trigger;

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == true)
            trigger.StartDialogue();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pressPanel.SetActive(true);
            Debug.Log("El boton F esta desactivado");
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pressPanel.SetActive(false);
            Debug.Log("El boton F esta desactivado");
            playerIsClose = false;         
        }
    }
}
