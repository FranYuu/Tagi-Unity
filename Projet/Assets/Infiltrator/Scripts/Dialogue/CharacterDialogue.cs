using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class CharacterDialogue : MonoBehaviour
{
    public List<Texture2D> Char;
    private String ResPath= "Assets/Infiltrator/Renderer/CharacterProfile/";
    private byte[] fileData;
    public GUIStyle customGuCharacter;
    //private String ResPath = "Dialogue/Character/";

    void OnGUI()
    {
        GUI.depth = 2;
        GUI.Box(new Rect(0, Screen.height/5, Screen.width / 4, Screen.height * 2 / 3), Char[0], customGuCharacter);
        GUI.Box(new Rect(Screen.width / 3, Screen.height / 5, Screen.width / 3, Screen.height * 2 / 3), Char[1], customGuCharacter);
        GUI.Box(new Rect(Screen.width*2/ 3, Screen.height / 5, Screen.width / 3, Screen.height * 2 / 3), Char[2], customGuCharacter);
    }
    public void addImage(string location,string name){
        String path = ResPath+ name + ".png";
        int position=0;
        switch (location)
        {
            case "L":
                position = 0;
                break;
            case "C":
                position = 1;
                break;
            case "R":
                position = 2;
                break;
        }

        if (File.Exists(path))
        {
            fileData = File.ReadAllBytes(path);
            Char[position] = new Texture2D(2, 2);
            Char[position].LoadImage(fileData); //..this will auto-resize the texture dimensions.
        }
        else
        {
            path = ResPath + "unknown.png";
            fileData = File.ReadAllBytes(path);
            Char[position] = new Texture2D(2, 2);
            Char[position].LoadImage(fileData);
        }


            
    }
    public void eraseAll()
    {
        for (int i = 0; i < Char.Count; i++)
            Char[i] = null;

    }
}
