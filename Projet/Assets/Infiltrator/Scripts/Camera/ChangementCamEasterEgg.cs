using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangementCamEasterEgg : MonoBehaviour
{
    public CameraControl camera;
    public bool doubleCollisionFix;
    private bool used;
    int previous, next;

    void Start()
    {
        doubleCollisionFix = false;
        used = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !doubleCollisionFix)
        {
            doubleCollisionFix = true;
            if (collision.transform.position.x > transform.position.x && !used)
            {

                camera.moveTo(next);

                used = true;
            }

            else if (collision.transform.position.x < transform.position.x && used)
            {

                camera.moveTo(previous);
                used = false;
            }

        }
        else
            doubleCollisionFix = false;
    }
}