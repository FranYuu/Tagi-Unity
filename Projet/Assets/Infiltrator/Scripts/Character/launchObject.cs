using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launchObject : MonoBehaviour
{

    public LayerMask surface;
    public GameObject objToThrow;

    private GameObject objThrown;
    private Collider2D objCollider;

    private Vector2 objPosition;
    private Vector2 objEndPosition;
    private Vector2 objHighPosition;

    private Character character;

    private float count = 0.0f;
    private int manacost = 10;
    private bool travel;
    private int right;
    private bool launch = true;

    // Use this for initialization
    void Start()
    {
        character = GetComponent<CharacterControllerPhysics2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0 && Time.time != pauseMenu.timePaused && !launch)
        {
            if (objThrown == null) count = 0.0f;
            else
                if (objCollider.IsTouchingLayers(surface)) travel = false;

            if(character.mana - manacost >= 0)
            {
                if (Input.GetButtonDown("Throw") && objThrown == null)
                {
                    right = CharacterControllerPhysics2D.facingRight ? 1 : -1;

                    objPosition = new Vector2(character.transform.position.x + right * 0.9f, character.transform.position.y + 0.3f);
                    objEndPosition = new Vector2(character.transform.position.x + right * 50, character.transform.position.y - 60);
                    objHighPosition = objPosition + (objEndPosition - objPosition) / 2 + Vector2.up * 40;

                    Throw();
                }
            }
           
            if (objThrown != null && travel)
            {
                Vector2 vec1 = Vector2.Lerp(objPosition, objHighPosition, count);
                Vector2 vec2 = Vector2.Lerp(objHighPosition, objEndPosition, count);
                objThrown.transform.position = Vector2.Lerp(vec1, vec2, count);

                count += 0.8f * Time.deltaTime;
            }
        }
    }

    void Throw()
    {
        count = 1.5f * Time.deltaTime;
        objThrown = Instantiate(objToThrow, objPosition, Quaternion.identity);
        objCollider = objThrown.GetComponent<Collider2D>();
        travel = true;
        character.mana -= manacost;
    }

    public void setLaunch(bool launch)
    {
        this.launch = launch;
    }
}
