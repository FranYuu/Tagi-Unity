using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Teleportation : Pouvoir
{
    public GameObject pointTeleport;
    public GameObject zone;
    public float distanceMax;

    private Character chara;
    private UIScript ui;
    private bool tp = true;

    void Start()
    {
        chara = GetComponent<Character>();
        ui = GetComponent<UIScript>();

        ui.hideTeleportation();
        
    }

    //Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && Time.time != pauseMenu.timePaused && !tp)
        {
            if (chara.mana != 0)
            {
                if (Time.time > coolDown + lastUsed)
                {
                    ui.TPtext.gameObject.SetActive(true);

                    if (Time.time > coolDown + lastUsed + 2)
                        ui.hideTeleportation();

                    if (chara.mana - manaCost >= 0)
                    {
                        if (Input.GetButton("Blink")) //si ce bouton permet de montrer la zone .
                        {
                            MontrerZone();//affiche la zone de téléportation
                            PositionTp();//affiche le curseur pour se téléporter
                        }
                        if (Input.GetButtonUp("Blink"))//cache la zone 
                        {
                            CacherZone();
                            Tp();//téléporte le personnage à la position du curseur
                            gameObject.SendMessage("playBlinkSound");
                        }
                    }
                }
                else
                    ui.showTeleportation();
            }
            else
                ui.hideTeleportation();
        }
        else
        {
            CacherZone();
        }
    }

    void Tp() //Téléporte le personnage à la position du curseur
    {
        this.transform.position = pointTeleport.transform.position;
        lastUsed = Time.time;
        ui.TPslider.value = 0f;
        ui.TPtext.gameObject.SetActive(false);
        chara.mana -= manaCost;
    }

    void MontrerZone()
    {
        float diametre = distanceMax * 0.8f; //Multiplication à changer en fonction de la distance max 
        zone.transform.localScale = new Vector3(diametre, diametre, -10);
    }

    void CacherZone()
    {
        zone.transform.localScale = new Vector3(0, 0, 0);
        pointTeleport.transform.localScale = new Vector3(0, 0, 0);

    }

    void PositionTp() //Place le curseur correctement et l'affiche
    {
        float tailleAfficher = 0.5f;
        pointTeleport.transform.localScale = new Vector3(tailleAfficher, tailleAfficher, 0);
        pointTeleport.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10)); //Place le curseur au niveau de la souris
        double distanceEntrePoints = Math.Sqrt((this.transform.position.x - pointTeleport.transform.position.x) * (this.transform.position.x - pointTeleport.transform.position.x) + (this.transform.position.y - pointTeleport.transform.position.y) * (this.transform.position.y - pointTeleport.transform.position.y));

        if (distanceEntrePoints < distanceMax)
            pointTeleport.transform.position = pointTeleport.transform.position; //Place le curseur au niveau de la souris si elle est dans la zone

        else //Si la souris est en dehors de la zone
        {
            double angle = Math.Asin((pointTeleport.transform.position.y - this.transform.position.y) / distanceEntrePoints);
            double x = distanceMax * Math.Cos(angle); //Calcul de la coordonée X du point d'intersection entre le segment et le cercle
            double y = distanceMax * Math.Sin(angle); //Calcul de la coordonée Y du point d'intersection entre le segment et le cercle

            if (pointTeleport.transform.position.x < this.transform.position.x) //Si le curseur est à gauche du joueur on multiplie par -1 pour avoir la bonne coordonée
                x *= -1;

            pointTeleport.transform.position = new Vector3((float)(this.transform.position.x + x), (float)(this.transform.position.y + y), 10); //Place le curseur à l'extrémité du cercle si la souris est en dehors de la zone
        }
    }

    public void setTP(bool tp)
    {
        this.tp=tp;
    }
}
