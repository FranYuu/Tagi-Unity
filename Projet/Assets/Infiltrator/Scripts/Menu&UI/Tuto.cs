using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tuto : MonoBehaviour
{
    public List<GameObject> waypoints;
    public DialogueManager dialogue;
    public GameObject wall;
    private int enemies;

    void Start()
    {
        GetComponent<Teleportation>().enabled = false;
        GetComponent<ProjectileLaunch>().enabled = false;
        GetComponent<Melee>().enabled = false;

        //Premier dialogue
        dialogue.StartDialogue();
    }
    
    void Update()
    {
        if (transform.position.x >= waypoints[0].transform.position.x && waypoints[0].GetComponent<Hint>().used == false)
        {
            waypoints[0].GetComponent<Hint>().used = true;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[1].transform.position.x && waypoints[1].GetComponent<Hint>().used == false)
        {
            waypoints[1].GetComponent<Hint>().used = true;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[2].transform.position.x && waypoints[2].GetComponent<Hint>().used == false)
        {
            waypoints[2].GetComponent<Hint>().used = true;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[3].transform.position.x && waypoints[3].GetComponent<Hint>().used == false)
        {
            GetComponent<Melee>().enabled = true;
            waypoints[3].GetComponent<Hint>().used = true;
            enemies = 1;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[4].transform.position.x && waypoints[4].GetComponent<Hint>().used == false)
        {
            GetComponent<ProjectileLaunch>().enabled = true;
            GetComponent<Melee>().enabled = false;
            waypoints[4].GetComponent<Hint>().used = true;
            enemies = 3;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[5].transform.position.x && waypoints[5].GetComponent<Hint>().used == false)
        {
            wall.SetActive(true);
            GetComponent<ProjectileLaunch>().enabled = false;
            GetComponent<Teleportation>().enabled = true;
            waypoints[5].GetComponent<Hint>().used = true;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[6].transform.position.x && waypoints[6].GetComponent<Hint>().used == false)
        {
            GetComponent<Teleportation>().enabled = false;
            GetComponent<Melee>().enabled = true;
            waypoints[6].GetComponent<Hint>().used = true;
            enemies = 1;


            dialogue.StartDialogue();
        }
        else if (transform.position.x >= waypoints[7].transform.position.x && waypoints[7].GetComponent<Hint>().used == false)
        {
            waypoints[7].GetComponent<Hint>().used = true;


            dialogue.StartDialogue();
        }

        //Voir le endPoint pour le dixième dialogue
    }

    public void NotifyEnemyDeath()
    {
        enemies--;
    }
}
