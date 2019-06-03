using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class contactSoundRange : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Enemy")
            gameObject.SendMessageUpwards("foundAnEnnemy",col);
    }
}
