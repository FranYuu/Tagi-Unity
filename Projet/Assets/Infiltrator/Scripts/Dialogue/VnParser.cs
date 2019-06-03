using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class VnParser : MonoBehaviour {
    List<DialogueLine> lines ;
    private int nombreElementARecupere = 4;
    private char startingCharacterContent = '\"';
    private string fileName="Dialogue.txt";
    private string DirDialoguePath= "Assets/Infiltrator/Dialogue/";
    //private String DirDialoguePath = "./Dialogue/";

    /// <summary>
    /// Les cookie c'est bon
    /// </summary>
    public void LoadDialogue() {

        lines = new List<DialogueLine>();
        string line;
        StreamReader r = new StreamReader(DirDialoguePath+fileName, System.Text.Encoding.UTF7);
        using (r) {
            do
            {
               line = r.ReadLine();
                if (line != null){
                    List<string> ligne = new List<string>();
                    string[] LigneParsed = line.Split(',');
                    foreach (string value in LigneParsed)
                    {
                        ligne.Add(value);
                    }
                    int i = 0;
                    while (ligne.Count>nombreElementARecupere) 
                    {
                        if (ligne[i][0] == startingCharacterContent)
                        {
                            ligne[i] = String.Concat(ligne[i], ligne[i + 1]);
                            ligne.Remove(ligne[i + 1]);
                        }
                        else
                            i++;
                        if (i > ligne.Count)
                            break;
                    }
                    lines.Add(new DialogueLine(ligne[0], ligne[1], ligne[2], ligne[3]));

                }
            }
            while (line != null);
            r.Close();
        }
    }
    public List<DialogueLine> GetDialoguesLine() {
        return lines;
    }
    public void setFileName(string fileName) {
        this.fileName = fileName+".txt";
    }
}
