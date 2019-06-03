using UnityEngine;
using System.Collections;
using System;

public class FieldOfViewEnemy : FOV
{
    public GameObject sourceVision;


    //variable qui définit la portée du cône
    public float lgVisionEnnemy = 10;
    public float heightVisionEnnemy = 8;
    private float angleVisionEnnemy = 1;


    //boolean qui indique si l'ennemi est tourné à droite ou à gauche
    public bool facingRight;

    private bool isRotateRight;
    private bool isRotateLeft;

    private int goodRotate;

    private CharacterControllerPhysics2D chara;

    new void Start()
    {

        light.transform.localScale = new Vector3(heightVisionEnnemy / transform.localScale.x * 2,
                                                 lgVisionEnnemy / transform.localScale.y / 2,
                                                 light.transform.localScale.z / transform.localScale.z);

        lgVision = lgVisionEnnemy;
        heightVision = heightVisionEnnemy;
        angleVision = angleVisionEnnemy;

        //facingRight = false;
        source = sourceVision;

        isRotateRight = false;
        isRotateLeft = false;
        base.Start();
        chara = player.GetComponent<CharacterControllerPhysics2D>();


    }

    new void Update()
    {
        underLight = chara.GetUnderLight();


        rotateFieldOfView();        
        hitInfo = calcRaycast();


        if (hitInfo.transform.position.x - transform.position.x > 0 && facingRight
            || hitInfo.transform.position.x - transform.position.x < 0 && !facingRight)
        {
            base.Update();
        }
    }


    public void IncreaseFieldLevel1()
    {
        lgVision = lgVision * 1.3f;
        light.transform.localScale = new Vector3(transform.localScale.x,
                                                 goodRotate * (lgVision / transform.localScale.y / 2),
                                                 transform.localScale.z);

    }

    public void IncreaseFieldLevel2()
    {
        lgVision = lgVision * 1.8f;
        heightVision = heightVision * 1.5f;
        light.transform.localScale = new Vector3(goodRotate * (heightVision / transform.localScale.x * 2),
                                                 goodRotate * (lgVision / transform.localScale.y / 2),
                                                 transform.localScale.z);
    }

    protected override void ComportementDetection()
    {
        //Mouvement de l'ennemi vers le joueur
        gameObject.SendMessage("GoToPlayer");

    }

    protected override void PlusAdistance()
    {
        gameObject.SendMessage("WaitAndGo");
    }

    private void rotateFieldOfView()
    {
        if (facingRight)
        {
            isRotateLeft = false;
            if (!isRotateRight)
            {
                isRotateRight = true;
                light.transform.localRotation = Quaternion.Euler(0, 0, -90);

            }

            lgVision = Math.Abs(lgVision); //la vision est positive, tourné vers la droite
            goodRotate = 1;
        }
        //Si l'ennemi est tourné vers la gauche
        else
        {
            isRotateRight = false;
            if (!isRotateLeft)
            {
                isRotateLeft = true;
                light.transform.localRotation = Quaternion.Euler(0, 0, 90);
            }
            lgVision = -Math.Abs(lgVision);//la vision est négative, tourné vers la gauche
            goodRotate = -1;
        }
    }
}

