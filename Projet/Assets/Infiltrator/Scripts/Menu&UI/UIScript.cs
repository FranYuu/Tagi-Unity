using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

    // Le personnage
    Character character;

    // UI Mana
    public Slider manaSlider;
    public Text manaText;

    // UI CD Teleportation
    public Slider TPslider;
    public Image TPimage;
    public Text TPtext;

    // UI Munition
    public Text munText;

    // UI Nombre d'ennemies
    public Text nbEnemyText;
    public static int nbEnemy;

    // Use this for initialization
    void Start () {
        character = GetComponent<Character>();

        manaSlider.transform.position = new Vector3(150, Screen.height - 40);
        manaText.transform.position = new Vector3(80, Screen.height - 70);

        TPslider.transform.position = new Vector3(500, Screen.height - 30);
        TPimage.transform.position = new Vector3(340, Screen.height - 70);
        TPtext.transform.position = new Vector3(490, Screen.height - 70);

        munText.transform.position = new Vector3(95, Screen.height - 90);
        nbEnemyText.transform.position = new Vector3(Screen.width - 70, Screen.height - 40);

        nbEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
	
	// Update is called once per frame
	void Update () {
        manaSlider.value = character.mana;
        manaText.text = "Mana: " + character.mana.ToString();
        if (character.mana == 0) manaSlider.gameObject.SetActive(false);

        munText.text = "Munitions: " + character.munition.ToString();
        nbEnemyText.text = "Ennemies: " + nbEnemy.ToString();
    }

    // Cache l'UI de la Teleportation
    public void hideTeleportation()
    {
        TPslider.gameObject.SetActive(false);
        TPimage.gameObject.SetActive(false);
        TPtext.gameObject.SetActive(false);
    }

    // Affiche l'UI de la Teleportation
    public void showTeleportation()
    {
        TPimage.gameObject.SetActive(true);
        TPslider.gameObject.SetActive(true);
        TPslider.value = Mathf.MoveTowards(TPslider.value, 5.0f, Time.deltaTime);
    }
}
