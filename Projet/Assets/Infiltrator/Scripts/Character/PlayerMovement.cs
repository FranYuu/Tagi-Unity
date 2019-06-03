using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Les attributs "public" peuvent être directement modifiés sur la scène

    public float maxSpeed = 275f;
    public float jumpSpeed = 12f;
    [Range(0,1)] public float crouchSpeed = 0.5f; //La vitesse du personnage lorsqu'il est accroupi.
    //La Range(0,1) permet d'éviter les valeurs ou le personnage serait accéléré lorsqu'il est accroupi ou qu'il ait une vitesse négative.

    //Correspond au objets testant la proximité du sol ou du plafond
    public Transform checkIfGround;
    public Transform checkIfCeiling;

    public Collider2D crouchDisableCollider; //Collider précisé qui sera désactivé lors de l'accroupissement du personnage
    public LayerMask ground; //Présice le Layer qui sera considéré comme le sol

    //Booléens testant les différents états du personnage
    public static bool facingRight = true;
    public static bool canMove = true;
    private bool onGround = false;
    private bool crouch = false;

    private Rigidbody2D character; //Sert à accéder au Rigidbody2D du personnage

    //Taille des "cercles" permettant de détecter le sol ou le plafond
    private readonly float groundRadius = 0.4f;
    private readonly float ceilingRadius = 0.4f;

    void OnEnable()
    {
        character = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Détecte le sol dans un cercle autour de l'objet checkIfGround
        onGround = Physics2D.OverlapCircle(checkIfGround.position, groundRadius, ground);
        float move = Input.GetAxis("Horizontal");

        //Allourdi le saut, evite l'effet ballerine lors du air control
        if (!onGround)
            character.gravityScale = 4f;

        //Repasse sur une gravité normale
        else
            character.gravityScale = 1f;

        //Si le joueur n'est plus accroupi
        if (!crouch)
        {
            //Si le joueur est sous un plafond l'empêchant de se lever, il reste accroupi
            if (Physics2D.OverlapCircle(checkIfCeiling.position, ceilingRadius, ground))
            {
                crouch = true;
            }
           
        }

        //Regarde si le joueur peut se lever (boutton d'accroupissement relâché ET pas de plafond l'empêchant de se lever)
        if (!Input.GetButton("Crouch") && !Physics2D.OverlapCircle(checkIfCeiling.position, ceilingRadius, ground))
            crouch = false;

        //Ralenti le mouvement du joueur si il est accroupi et désactive le collider précisé que l'on veut désactiver
        if (crouch)
        {
            move = move * crouchSpeed;
            crouchDisableCollider.enabled = false;
        }
        else
            crouchDisableCollider.enabled = true;

        /*
         * A décommenter en cas d'erreur
        Vector3 localScale = transform.localScale;
        */

        if (move < 0 && canMove)
        {
            if (facingRight)
            {
                //Change l'orientation du joueur
                Flip();
                gameObject.SendMessage("GunFlip");
            }
        }
        else if (move > 0 && canMove)
        {
            if (!facingRight)
            {
                Flip();
                gameObject.SendMessage("GunFlip");
            }
        }

        /*
         * A décommenter en cas d'erreur
        if ((facingRight && (localScale.x < 0)) || (!facingRight && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
        */

        //Mouvement selon les secondes et NON les frames (deltaTime)
        if (canMove)
        {
            character.velocity = new Vector2((move * maxSpeed) * Time.deltaTime, character.velocity.y);
        }
        
    }

    void Update()
    {
        //Vérifie si le joueur peut sauter (sur le sol et appuie sur la touche de saut
        if (onGround && Input.GetButtonDown("Jump") && !crouch && canMove)
        {
            character.velocity = new Vector2(0, jumpSpeed);
        }

        //Appuyer légèrement sur la touche de saut "annule" le reste du saut, permettant de sauter plus haut lorsqu'on laisse appuyé plus longtemps
        else if (Input.GetButtonUp("Jump") && canMove)
        {
            if (character.velocity.y > 0)
            {
                character.velocity = new Vector2(0, character.velocity.y * 0.5f);
            }
        }

        //Regarde les inputs du joueur pour savoir si le personnage doit s'accroupir
        if (Input.GetButtonDown("Crouch") && canMove)
        {
            crouch = true;
        }
        else if (Input.GetButtonUp("Crouch") && canMove)
        {
            crouch = false;
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;

        //Rajouter ici les méthodes sur l'Animator
    }
}