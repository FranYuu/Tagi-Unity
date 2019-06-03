using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileContact : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Destroyable" || collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
        if (collision.tag != "NotHitByBullet")
        {
            Destroy(gameObject);
        }
    }
}
