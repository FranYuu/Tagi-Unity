using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class pauseMenu : MonoBehaviour {

    private bool paused = false;
    public static float timePaused;

	void Update () {

        //Correspond au bouton escape
        if (Input.GetButtonDown("Cancel"))
            paused = !paused;
        if (paused)
        {
            timePaused = Time.time;
            Time.timeScale = 0;
        }            
        else
            Time.timeScale = 1;
    }

    void OnGUI()
    {
        if (paused)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 - 100, 150, 50), "Continue"))
                paused = false;

            if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 00, 150, 50), "Restart Level"))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            if (GUI.Button(new Rect(Screen.width / 2 - 60, Screen.height / 2 + 100, 150, 50), "Exit to Main Menu"))
                SceneManager.LoadScene(0);
        }
    }
}
