using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    /// <summary>
    /// Les attributs "public" peuvent être directement modifiés sur la scène
    /// </summary>
    public float maxSpeed = 275f;
    public float jumpSpeed = 12f;
    public int mana = 100;
    public int munition = 30;

    //Pas encore implémenté, séléction des pouvoirs
    //List<Pouvoir> pouvoirs;

    // Use this for initialization
    void Start () {
        //pouvoirs = new List<Pouvoir>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
