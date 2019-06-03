using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint : MonoBehaviour {

    public int indexOfNextScene;
	private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            LoadByIndex(indexOfNextScene);
    }
    public void LoadByIndex(int index)
    {
        if (index != SceneManager.GetActiveScene().buildIndex && index != 0 && index < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(index);
    }
}
