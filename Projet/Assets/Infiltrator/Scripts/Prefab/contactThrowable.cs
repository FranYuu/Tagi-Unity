using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contactThrowable : MonoBehaviour {

    public LayerMask enemy;
    public LayerMask surface;
    public GameObject soundRange;
    public float soundRadius = 50f;
    private CircleCollider2D effectiveRange;
    private Collider2D objCollider;
    private Renderer soundSprite;
    private Renderer objRenderer;
    private Rigidbody2D soundBody;
    private int timePause = 2;

    private void Start()
    {
        objCollider = GetComponent<Collider2D>();
        objRenderer = GetComponent<Renderer>();
        effectiveRange = soundRange.GetComponent<CircleCollider2D>();
        soundSprite = soundRange.GetComponent<Renderer>();
        soundBody = soundRange.GetComponent<Rigidbody2D>();
        soundRange.transform.localScale = new Vector2(soundRadius,soundRadius);
        effectiveRange.radius = 0.83f; //Correspond au rapport des radius. S'adapte en fonction du soundRadius
        effectiveRange.enabled = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (timePause == 0)
        {
            Destroy(gameObject);
            StopCoroutine("CountDown");
            timePause = 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Ce if permet de ne passer que une seule fois dans cette méthode
        //Sinon elle sera appelé a chaque contact de surface, potentiellement donc plusieurs fois si ça touche le player, puis le sol, etc..
        if (objCollider.IsTouchingLayers(surface) && objRenderer.enabled)
        {
            MakeNoise();
            StartCoroutine("CountDown");
        }
            
    }

    private void MakeNoise()
    {
        soundSprite.enabled = true;
        effectiveRange.enabled = true;
        soundBody.bodyType = RigidbodyType2D.Static;
        objRenderer.enabled = false;
    }

    public void foundAnEnnemy(Collider2D en)
    {
        en.gameObject.SendMessage("GoToPoint", soundSprite.transform);     
    }

    IEnumerator CountDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timePause--;
        }
    }
}
