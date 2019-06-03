using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCollide : MonoBehaviour
{
    public Canvas canvas;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        Animator anim = canvas.GetComponent<Animator>();
        anim.SetTrigger("Death");

        //Attendre 1 secondes
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
