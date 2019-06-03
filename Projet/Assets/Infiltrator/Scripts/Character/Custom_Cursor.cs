using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Custom_Cursor : MonoBehaviour
{
    public Transform custom_cursor;
    private Vector2 cursorDirection;
    private Vector2 oldPos;
    private Vector2 oldDirection;
    private float lastMove = 0f;

    // Start is called before the first frame update
    void Start()
    {
        oldPos = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        cursorDirection = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        if (oldPos != cursorDirection)
        {
            lastMove = Time.time;
        }

        if ((Time.time > 3f + lastMove) && !GetComponent<ProjectileLaunch>().GetFiring())
        {
            custom_cursor.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            custom_cursor.GetComponent<Renderer>().enabled = true;
        }

        custom_cursor.position = cursorDirection;

        oldPos = cursorDirection;
    }
}
