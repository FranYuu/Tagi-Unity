using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangementCam : MonoBehaviour
{
    public CameraControl camera;
    private bool used;

    void Start()
    {
        used = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.transform.position.x > transform.position.x && !used)
            {

                camera.moveForward();
                used = true;
            }

            else if (collision.transform.position.x < transform.position.x && used)
            {
                camera.moveBackward();
                used = false;
            }

        }
    }
}