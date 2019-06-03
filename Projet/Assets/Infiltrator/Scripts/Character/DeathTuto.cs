using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTuto : MonoBehaviour
{
    public GameObject enemy1;
    public GameObject enemy2;
    public DialogueManager dialogue;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == enemy1)
        {
            //Message de mort 1
            Destroy(collision.gameObject);
            dialogue.StartDialogue();
        }
        if (collision.gameObject == enemy2)
        {
            //Message de mort 2
            Destroy(collision.gameObject);
            dialogue.StartDialogue();
        }
    }
}
