using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fin : MonoBehaviour {

    private bool fini;
    private bool CR_used;
    public int timePause = 5;
    private GUIStyle guiStyle = new GUIStyle();
    public DialogueManager dialogue;

    void Start() {
        fini = false;
        CR_used = false;
        guiStyle.fontSize = 30;
        guiStyle.normal.textColor = Color.white;
    }

    void Update() {
        if (timePause == 0)
        {
            StopCoroutine("CountDown");
            Application.Quit();
        }
        if(fini &&  !CR_used)
        {
            StartCoroutine("CountDown");
            CR_used = true;
        }

    }

    void OnGUI()
    {
        if (fini)
        {
            GUI.Label(new Rect(Screen.width / 2 - 60, Screen.height / 2 - 60, 150, 50), "Gagné",guiStyle);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" ) { 
            fini = true;
            dialogue.StartDialogue();
        }

    }

    IEnumerator CountDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timePause--;
        }
    }
}
