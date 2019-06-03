using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour {
    private VnParser parser = new VnParser();
    private int  numeroLigne;
    private List<DialogueLine> currentDialogue;
    public List<string> file;
    private int numeroFile;
    public DialogueBox dialogueBox;
    public CharacterDialogue dialogueChar;
    private  bool finisheded;
    private bool playing=false;
    public CharacterControllerPhysics2D player;
    public ProjectileLaunch piouPiou;
    public Teleportation teleport;
    public launchObject grenade;

    // Use this for initialization
    void Start() {

        parser.setFileName(file[0]);
        numeroFile = 0;
        LoadParser();
        numeroLigne = 0;
        finisheded = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonUp("Next")&&playing)
        { 
            NextLine();
        }
    } 
    public void LoadParser() {
        parser.LoadDialogue();
        currentDialogue = parser.GetDialoguesLine();
    }
    public void StartDialogue()
    {
        CharacterControllerPhysics2D.canMove = false;
        piouPiou.setShoot(true);
        teleport.setTP(true);
        grenade.setLaunch(true);
        if (finisheded) {
            finisheded = false;
            NextFile();
        }
        dialogueBox.startDialogue();
        playing = true;
        numeroLigne = 0;
        dialogueBox.SetCurrentLine(currentDialogue[numeroLigne]);
        dialogueChar.addImage(currentDialogue[numeroLigne].getScreenPose(), currentDialogue[numeroLigne].getName());
    }

    public void NextLine(){
        if (numeroLigne < currentDialogue.Count-1)
        {
            numeroLigne++;
            dialogueBox.SetCurrentLine(currentDialogue[numeroLigne]);
            dialogueChar.addImage(currentDialogue[numeroLigne].getScreenPose(), currentDialogue[numeroLigne].getName());
        }
        else
        {
            player.enabled = true;
            dialogueBox.EndDialogue();
            dialogueChar.eraseAll();
            finisheded = true;
            playing = false;
            CharacterControllerPhysics2D.canMove = true;
            StartCoroutine(Wait());
        }
    }
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        piouPiou.setShoot(false);
        teleport.setTP(false);
        grenade.setLaunch(false);
    }
    public void NextFile()
    {
        if (numeroLigne < currentDialogue.Count)
        {
            numeroFile++;
            parser.setFileName(file[numeroFile]);
            LoadParser();
            numeroLigne = 0;
        }
    }
}
