using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float maxSpeedForPlayer = 30f;
    public AudioSource audioSrc;
    public AudioClip sneakClip;
    public AudioClip encounterClip;
    public AudioClip alertClip;
    public List<GameObject> waypoints;
    public GameObject player;
    private GameObject pointToGo;
    private float speed;
    public int timePause = 2;
    public int timePauseAfterDetect = 5;
    private int backupTimePause;
    private int cpt = 0;
    private bool canMove = true;
    private bool inSound = false;
    private bool vuPlayer = false;
    private bool hasToStop = false;
    private bool CR_running = false;
    private bool detected = false;
    private Animator anim;
    private Animator anim2;
    private Transform cible;
    private SpriteRenderer enSprite;
    private FieldOfViewEnemy FOV;
    private float oldPos;
    private int lvlDetection;

    void Start () {
        pointToGo = new GameObject("PointToGo");
        //pointToGo.transform.position = new Vector2();
        Time.timeScale = 1;
        lvlDetection = 0;
        backupTimePause = timePause;
        oldPos = transform.position.x;
        speed = maxSpeed;
        anim = GetComponent<Animator>();
        anim2 = player.GetComponent<Custom_Cursor>().custom_cursor.GetComponent<Animator>();
        enSprite = GetComponent<SpriteRenderer>();
        FOV = GetComponent<FieldOfViewEnemy>();
        if (waypoints.Count == 0) throw new MissingComponentException(); //Sécurité si oubli de rajouter un waypoint minimal
    }
 
    void FixedUpdate()
    {
        if (hasToStop && !CR_running)
            StartCoroutine("CountDown");

        if (vuPlayer)
            cible = player.transform;
        else if (inSound)
            cible = pointToGo.transform; //Le centre de la zone de son
        else if (waypoints.Count != 0)
            cible = waypoints[cpt].transform;
        else
            cible = transform; //La position de l'ennemi, donc immobile

        if ((transform.position.x == pointToGo.transform.position.x) && (transform.position.y != pointToGo.transform.position.y))
        { //Check si l'ennemi est arrivé au centre de le zone de son. Si oui, il se stoppe
            PauseMovment();
            Destroy(pointToGo);
            pointToGo = new GameObject("PointToGo");
        }

        if (timePause == 0)
        {
            if (audioSrc.clip != sneakClip && vuPlayer)
                playSneakTheme();
            StopTimer(true);
        }

        if (oldPos < transform.position.x && !FOV.facingRight)
        {
            FOV.facingRight = true;
            enSprite.flipX = !enSprite.flipX;
        }
        if (oldPos > transform.position.x && FOV.facingRight)
        {
            FOV.facingRight = false;
            enSprite.flipX = !enSprite.flipX;
        }

        oldPos = transform.position.x;

        anim.SetBool("canMove", canMove);
        anim.SetBool("vuPlayer", vuPlayer);
        anim.SetBool("hasToStop", hasToStop);

        if (canMove)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(cible.position.x,transform.position.y), speed * Time.deltaTime);
    }

    void StopTimer(bool disableGoTo)
    {
        StopCoroutine("CountDown");
        canMove = true;
        if (waypoints.Count == 1 && !vuPlayer && !inSound) canMove = false; //Pour que le garde se tienne droit dans le cas d'un seul waypoint
        hasToStop = false;
        if (disableGoTo)
        {
            vuPlayer = false;
            inSound = false;
        }
        CR_running = false;
        timePause = backupTimePause;
        speed = maxSpeed;
    }

    void NextWaypoint(GameObject call)
    {
        //if (call.GetInstanceID() == waypoints[cpt].GetInstanceID()) Compare les instance ID
        if (call == waypoints[cpt])
        {
            if(!vuPlayer) hasToStop = true;
            if (cpt < waypoints.Count - 1)
            {
                cpt++;
            }
            else
            {
                cpt = 0;
            }
        }
    }

    void GoToPlayer()
    {
        if (audioSrc.clip == sneakClip)
        {
            audioSrc.PlayOneShot(alertClip, 1);
            audioSrc.clip = encounterClip;
            audioSrc.PlayDelayed(0.8f);
        }
        if (lvlDetection < 2)
        {
            gameObject.SendMessage("IncreaseFieldLevel2");
            lvlDetection = 2;
        }
        anim2.SetBool("playerDetected", true);
        vuPlayer = true;
        StopTimer(false);
        speed = maxSpeedForPlayer;
    }

    void GoToPoint(Transform pointPosition)
    {
        pointToGo.transform.SetPositionAndRotation(pointPosition.position, Quaternion.identity);
        inSound = true;
        if (lvlDetection == 0)
        {
            gameObject.SendMessage("IncreaseFieldLevel1");
            lvlDetection = 1;
        }
        StopTimer(false);
    }

    void WaitAndGo()
    {
        if (vuPlayer && !CR_running)
        {
            PauseMovment();
        }
    }

    void PauseMovment()
    {
        timePause = timePauseAfterDetect;
        hasToStop = true;
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            CR_running = true;
            canMove = false;
            yield return new WaitForSeconds(1);
            timePause--;          
        }
    }

    public int GetLvlDetection()
    {
        return lvlDetection;
    }

    private void playSneakTheme()
    {
        if (audioSrc.clip != sneakClip)
        {
            audioSrc.clip = sneakClip;
            audioSrc.PlayDelayed(0.8f);
            anim2.SetBool("playerDetected", false);
        }
    }

    private void OnDestroy()
    {
        UIScript.nbEnemy--;
        if (audioSrc != null)
            playSneakTheme();
    }
}

