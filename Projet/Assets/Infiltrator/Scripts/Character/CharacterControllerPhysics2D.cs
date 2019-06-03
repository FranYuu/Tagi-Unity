using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterControllerPhysics2D : Character
{
    /// <summary>
    /// Les attributs "public" peuvent être directement modifiés sur la scène
    /// </summary>

    [Range(0, 1)] public float crouchSpeed = 0.5f; //La vitesse du personnage lorsqu'il est accroupi.
                                                   //La Range(0,1) permet d'éviter les valeurs ou le personnage serait accéléré lorsqu'il est accroupi ou qu'il ait une vitesse négative.

    //Correspond aux objets testant la proximité du sol ou du plafond.
    public Transform checkIfGround;
    public Transform checkIfCeiling;

    public AudioClip gunClip;
    public AudioClip blinkClip;

    public LayerMask ground; //Présice le Layer qui sera considéré comme le sol.

    //Booléens testant les différents états du personnage.
    public static bool facingRight = true;
    public static bool canMove = true;
    public static bool canFlip = true;
    private bool onGround = false;
    private bool crouch = false;
    private bool wasCrouching = false;

    protected Rigidbody2D character; //Sert à accéder au Rigidbody2D du personnage

    //Taille des cercles de test permettant de détecter le sol ou le plafond
    private float groundRadius = 0.5f;
    private float ceilingRadius = 0.2f;

    private BoxCollider2D bc;
    private Vector2 sizeCrouch, sizeNormal, sizeRunning, offsetCrouch, offsetNormal;
    private SpriteRenderer playerSprite;
    private Animator anim;
    private AudioSource audioSrc;
    protected bool underLight;

    void OnEnable()
    {
        character = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        bc = GetComponent<BoxCollider2D>();
        sizeNormal = new Vector2(bc.size.x, bc.size.y);
        offsetNormal = new Vector2(bc.offset.x, bc.offset.y);
        sizeRunning = new Vector2(7, bc.size.y);
        sizeCrouch = new Vector2(10, 5);
        offsetCrouch = new Vector2(bc.offset.x, -3);
        Cursor.visible = false;
    }

    void FixedUpdate()
    {
        //Détecte le sol dans un cercle autour de l'objet checkIfGround
        onGround = Physics2D.OverlapCircle(checkIfGround.position, groundRadius, ground);

        //Allourdi le saut, evite l'effet ballerine lors du air control
        if (!onGround)
            character.gravityScale = 4f;

        //Repasse sur une gravité normale
        else
            character.gravityScale = 1f;
        if (canMove)
        {
            float move = Input.GetAxis("Horizontal");
            //Si le joueur n'est plus accroupi
            if (!crouch)
            {
                //Si le joueur est sous un plafond l'empêchant de se lever, il reste accroupi
                if (Physics2D.OverlapCircle(checkIfCeiling.position, ceilingRadius, ground) && wasCrouching)
                {
                    crouch = true;
                }
                wasCrouching = false;
            }

            //Regarde si le joueur peut se lever (boutton d'accroupissement relâché ET pas de plafond l'empêchant de se lever)
            if (!Input.GetButton("Crouch") && !Physics2D.OverlapCircle(checkIfCeiling.position, ceilingRadius, ground))
                crouch = false;

            //Ralenti le mouvement du joueur si il est accroupi et désactive le collider précisé que l'on veut désactiver
            if (crouch)
            {
                move = move * crouchSpeed;
                bc.size = sizeCrouch;
                bc.offset = offsetCrouch;
            }
            else if (Mathf.Abs(move)>0)
            {
                bc.size = sizeRunning;
                bc.offset = offsetNormal;
            }
            else
            {
                bc.size = sizeNormal;
                bc.offset = offsetNormal;
            }

            //Si le joueur peut se déplacer
            if (canMove)
            {
                //Tourner le sprite du joueur
                if ((move > 0 && !facingRight) || (move < 0 && facingRight))
                    Flip();

                //Mouvement selon les secondes et NON les frames (deltaTime)
                character.velocity = new Vector2((move * maxSpeed) * Time.deltaTime, character.velocity.y);
            }

            anim.SetBool("isMeleeAtk", false);
            anim.SetFloat("Speed", Mathf.Abs(move));
            anim.SetBool("isCrouching", crouch);
            anim.SetFloat("jumpSpeed", Mathf.Abs(character.velocity.y));
            if (character.velocity.y < 0) anim.SetBool("isFalling", true);
            else anim.SetBool("isFalling", false);

        }
    }

    void Update()
    {
        if (canMove)
        {
            //Vérifie si le joueur peut sauter (sur le sol et appuie sur la touche de saut)
            if (onGround && Input.GetButtonDown("Jump") && !crouch)
            {
                character.velocity = new Vector2(0, jumpSpeed);
            }

            //Appuyer légèrement sur la touche de saut "annule" le reste du saut, permettant de sauter plus haut lorsqu'on laisse appuyé plus longtemps
            else if (Input.GetButtonUp("Jump"))
            {
                if (character.velocity.y > 0)
                {
                    character.velocity = new Vector2(0, character.velocity.y * 0.5f);
                }
            }

            //Regarde les inputs du joueur pour savoir si le personnage doit s'accroupir
            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
                wasCrouching = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }
        }
        else
        {
            character.velocity = new Vector2(0, character.velocity.y);
        }
    }

    private void Flip()
    {
        if (canFlip)
        {
            facingRight = !facingRight;
            playerSprite.flipX = !playerSprite.flipX;

            gameObject.SendMessage("GunFlip");
            gameObject.SendMessage("MeleeFlip");
        }
    }

    public bool GetUnderLight()
    {
        return underLight;
    }

    public void UnderLight(bool detection)
    {
        underLight = detection;
    }

    public void playGunSound()
    {
        audioSrc.PlayOneShot(gunClip, 1);
    }

    public void playBlinkSound()
    {
        audioSrc.PlayOneShot(blinkClip, 1);
    }

    public bool GetCrouch()
    {
        return crouch;
    }

    public void playAnimationGun(bool yn)
    {
        anim.SetBool("isFiring", yn);
    }
    public void playAnimationMelee()
    {
        anim.SetBool("isMeleeAtk", true);
    }


}
