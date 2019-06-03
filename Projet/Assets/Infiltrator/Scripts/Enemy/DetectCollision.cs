using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollision : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D call)
    {
        if (call.gameObject.tag == "Waypoint")
            gameObject.SendMessage("NextWaypoint",call.gameObject);
    }
}
