using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : MonoBehaviour {

    public float maxSpeed = 5f;
    public GameObject player;

	// Update is called once per frame
	void Update () {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, maxSpeed * Time.deltaTime);
    }
}
