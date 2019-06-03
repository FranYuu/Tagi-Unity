using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLine {
    private string name;
    private string content;
    private string art;
    private string screenPose;

    public DialogueLine(string name, string content, string art, string screenPose) {
        this.name = name;
        this.content = content;
        this.art = art;
        this.screenPose = screenPose;
    }
    public string getName() {
        return name;
    }
    public string getContent()
    {
        return content;
    }
    public void setContent(string content)
    {
       this.content=content;
    }
    public string getStart()
    {
        return art;
    }
    public string getScreenPose()
    {
        return screenPose;
    }
}

