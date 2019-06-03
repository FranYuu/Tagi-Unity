using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueStarter : MonoBehaviour
{
    public DialogueManager dialogue;
    private bool used;


    void Start()
    {
        used = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !used)
        {
            dialogue.StartDialogue();
            used = true;
        }
    }
}