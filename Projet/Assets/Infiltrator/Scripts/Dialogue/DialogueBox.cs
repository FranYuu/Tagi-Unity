using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class DialogueBox : MonoBehaviour
{
    private DialogueLine currentLine;
    public GUIStyle customGuiName;
    public GUIStyle customGuiContent;
    private Texture2D zone;
    public bool showing;
    // Use this for initialization
    public void Start() {
        guiSetup();

    }
    public void startDialogue()
    {
        showing = true;
    }
    public void SetCurrentLine(DialogueLine currentLine)
    {
        this.currentLine = currentLine;
    }
    public void EndDialogue()
    {
        currentLine = null;
        showing = false;
    }
    void OnGUI()
    {
        if (showing)
        {
            GUI.depth = 1;
            GUI.Box(new Rect(0, Screen.height * 2 / 3, Screen.width, Screen.width), currentLine.getContent(), customGuiContent);
            GUI.Box(new Rect(0, (Screen.height * 2 / 3) - 15, Screen.width / 3, 15), currentLine.getName(), customGuiName);
        }
    }
    public void guiSetup() {
        customGuiName.normal.textColor = Color.white;
        customGuiContent.normal.textColor = Color.white;
        customGuiName.padding.left = Screen.width / 15;
        customGuiContent.padding.left =  Screen.width / 10;
        customGuiContent.padding.top = Screen.height / 20;
        if (Screen.width <= 800)
        {
            customGuiName.fontSize = 25;
            customGuiContent.fontSize = 20;
        }
        else if (Screen.width <= 1600)
        {
            customGuiName.fontSize = 35;
            customGuiContent.fontSize = 30;
        }
        else {
            customGuiName.fontSize = 45;
            customGuiContent.fontSize = 40;
        }



    }
}