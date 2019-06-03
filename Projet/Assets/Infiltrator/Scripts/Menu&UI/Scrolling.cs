using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrolling : MonoBehaviour {
    float scrollSpeed = -2f;
    Vector2 startPos;

	// Use this for initialization
	void Start () {
        Cursor.visible = true;
        startPos = transform.position;
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
        float newPos = Mathf.Repeat(Time.time*scrollSpeed,75.68f);
        transform.position = startPos + Vector2.right * newPos;
	}
}
