using UnityEngine;
using System;

public class ProjectileLaunch : MonoBehaviour
{
    public float bulletSpeed = 30f;  //Vitesse de la balle
    [Range(0f, 10f)] public float fireRate = 1f;    //Cadence de tir limitée entre 0 et 10
    private float lastShot = 0f;    //Permet d'enregistrer le temps auquel on a lancé la dernière balle

    public float maxAngle = 70f;

    public GameObject bullet;       //Permet de renseigner la prefab de la balle

    public Transform cursor;        //Transform du curseur personnalisé
    public Transform gunTransform;  //Transform du pistolet

    //Lignes affichées à l'écran pour le champ de tir et la ligne de visée
    public LineRenderer aimLine;
    public LineRenderer higherVisionLine;
    public LineRenderer lowerVisionLine;

    //Raycast testant la collision avec un obstacle pour déterminer ou les lignes de visée et du champ de tir s'arrêtent.
    private RaycastHit2D aimHit;
    private RaycastHit2D higherAimHit;
    private RaycastHit2D lowerAimHit;

    private GameObject projectile;      //L'objet que l'on va lancé lors du tir

    private Vector2 direction;          //Direction du projectile
    private Vector2 cursorDirection;    //Direction du curseur
    private Vector2 oldPos;
    private Vector2 oldDirection;

    private bool firing = false;
    private bool hasFlip = false;
    private bool shoot = true;

    private CharacterControllerPhysics2D chara;

    //Initialisation de variables
    void Start()
    {
        Renderer fire_cursor = cursor.GetComponent<Renderer>();

        chara = GetComponent<CharacterControllerPhysics2D>();

        oldPos = new Vector2(0, 0);

        fire_cursor.enabled = false;
        CharacterControllerPhysics2D.facingRight = true;
        aimLine.enabled = false;
        higherVisionLine.enabled = false;
        lowerVisionLine.enabled = false;
    }

    //Appelée à chaque frame
    void Update()
    {
        if (Time.timeScale != 0 && Time.time != pauseMenu.timePaused && !chara.GetCrouch())
        {
            if (Time.time > fireRate + lastShot && chara.munition != 0 && !shoot)
            {
                if (Input.GetButton("Fire1") && Time.time > fireRate + lastShot)
                {
                    gameObject.SendMessage("playAnimationGun", true);
                    FollowCursor();
                    if (Input.GetButton("CancelFire"))
                    {
                        DesactivateFOV();
                    }
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    Fire();
                    gameObject.SendMessage("playGunSound");
                    gameObject.SendMessage("playAnimationGun", false);
                }
            }
        }
    }

    //Instantiation du projectile et de sa direction
    void Fire()
    {
        projectile = (GameObject)Instantiate(bullet, gunTransform.position, Quaternion.identity);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
        float rot_z = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        projectile.GetComponent<Transform>().rotation = Quaternion.Euler(0f, 0f, rot_z);

        DesactivateFOV();

        chara.munition -= 1;
    }

    //Visee du personnage
    void FollowCursor()
    {
        firing = true;

        cursorDirection = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        GetComponent<Custom_Cursor>().custom_cursor.GetComponent<Renderer>().enabled = true;
        GetComponent<Custom_Cursor>().custom_cursor.position = cursorDirection;
        direction = cursorDirection;
        direction = DirectionNormalized(direction);

        float distance = 10; //Distance du curseur par rapport au personnage
        float ClampX = distance * 0.50f;
        float ClampY = distance * 0.90f;

        double distanceEntrePoints = Math.Sqrt((this.transform.position.x - cursor.position.x) * (this.transform.position.x - cursor.position.x) + (this.transform.position.y - cursor.position.y) * (this.transform.position.y - cursor.position.y));
        double agl = Math.Asin((cursor.position.y - transform.position.y) / distanceEntrePoints);
        double x = distance * Math.Cos(agl);

        if (cursor.position.x < transform.position.x) // Si le curseur est à gauche du joueur on multiplie par -1 pour avoir la bonne coordonnée
        {
            x *= -1;
        }
        cursor.position = cursorDirection;

        if ((CharacterControllerPhysics2D.facingRight && x < 0) || (!CharacterControllerPhysics2D.facingRight && x > 0))
        {
            CharacterControllerPhysics2D.canFlip = true;
            this.SendMessage("Flip");
            hasFlip = true;
        }

        direction = cursor.position;
        direction = DirectionNormalized(direction);
        float angle = Vector2.Angle(Vector2.right, cursor.localPosition - gunTransform.localPosition);

        if (CharacterControllerPhysics2D.facingRight)
        {
            if (angle > maxAngle)
            {
                if (hasFlip)
                {
                    if (cursorDirection.y <= gameObject.transform.position.y)
                    {
                        direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90 + maxAngle)), Mathf.Cos(Mathf.Deg2Rad * (90 + maxAngle)));
                    }
                    else
                    {
                        direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90 - maxAngle)), Mathf.Cos(Mathf.Deg2Rad * (90 - maxAngle)));
                    }
                    oldDirection = direction;
                    hasFlip = false;
                }
                else
                {
                    direction = oldDirection; //new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90 - maxAngle)), Mathf.Cos(Mathf.Deg2Rad * (90 - maxAngle)));
                }
            }
            else
            {
                oldDirection = direction;
            }
        }
        else
        {
            if (angle < (180 - maxAngle))
            {
                if (hasFlip)
                {
                    if (cursorDirection.y <= gameObject.transform.position.y)
                    {
                        direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -(90 + maxAngle)), Mathf.Cos(Mathf.Deg2Rad * -(90 + maxAngle)));
                    }
                    else
                    {
                        direction = new Vector2(Mathf.Sin(Mathf.Deg2Rad * -(90 - maxAngle)), Mathf.Cos(Mathf.Deg2Rad * -(90 - maxAngle)));
                    }
                    oldDirection = direction;
                    hasFlip = false;
                }
                else
                {
                    direction = oldDirection;
                }
            }
            else
            {
                oldDirection = direction;
            }
        }

        Vector2 gunPosition = gunTransform.position;

        aimHit = Physics2D.Raycast(gunPosition, direction);

        if (CharacterControllerPhysics2D.facingRight)
        {
            higherAimHit = Physics2D.Raycast(gunPosition, new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90 - maxAngle)), Mathf.Cos(Mathf.Deg2Rad * (90 - maxAngle))));
            lowerAimHit = Physics2D.Raycast(gunPosition, new Vector2(Mathf.Sin(Mathf.Deg2Rad * (90 + maxAngle)), Mathf.Cos(Mathf.Deg2Rad * (90 + maxAngle))));
        }
        else
        {
            higherAimHit = Physics2D.Raycast(gunPosition, new Vector2(Mathf.Sin(Mathf.Deg2Rad * -(90 - maxAngle)), Mathf.Cos(Mathf.Deg2Rad * -(90 - maxAngle))));
            lowerAimHit = Physics2D.Raycast(gunPosition, new Vector2(Mathf.Sin(Mathf.Deg2Rad * -(90 + maxAngle)), Mathf.Cos(Mathf.Deg2Rad * -(90 + maxAngle))));
        }

        //Active les lignes de visée et de champ de vision
        ActivateFieldOfView();

        CharacterControllerPhysics2D.canFlip = false;
        GetComponent<Teleportation>().enabled = false;
    }

    //Active les lignes visibles à l'écran de champ de tir et de visée
    void ActivateFieldOfView()
    {
        aimLine.enabled = true;
        higherVisionLine.enabled = true;
        lowerVisionLine.enabled = true;

        aimLine.SetPosition(0, gunTransform.position);
        aimLine.SetPosition(1, aimHit.point);

        higherVisionLine.SetPosition(0, gunTransform.position);
        higherVisionLine.SetPosition(1, higherAimHit.point);

        lowerVisionLine.SetPosition(0, gunTransform.position);
        lowerVisionLine.SetPosition(1, lowerAimHit.point);
    }
    
    void DesactivateFOV()
    {
        lastShot = Time.time;

        aimLine.enabled = false;
        higherVisionLine.enabled = false;
        lowerVisionLine.enabled = false;

        firing = false;

        CharacterControllerPhysics2D.canFlip = true;
        if(!gameObject.scene.name.Equals("Tuto"))
            GetComponent<Teleportation>().enabled = true;
    }

    //Normalise la direction du projectile
    Vector2 DirectionNormalized(Vector2 direction)
    {
        direction = direction - (Vector2)gunTransform.position;
        direction.Normalize();
        return direction;
    }

    public bool GetFiring()
    {
        return firing;
    }

    public void setShoot(bool shoot)
    {
	    this.shoot = shoot;
    }

    //Inverse la position du pistolet et sa rotation en fonction du joueur.
    public void GunFlip()
    {
        gunTransform.localPosition = new Vector2(gunTransform.localPosition.x * -1, gunTransform.localPosition.y);
        gunTransform.localRotation = Quaternion.Inverse(gunTransform.localRotation);
    }
}

