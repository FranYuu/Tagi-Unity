using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretRoom : MonoBehaviour
{
    public GameObject enemy;
    public BoxCollider2D wall;
    private SpriteRenderer wallSprite;
    // Start is called before the first frame update
    void Start()
    {
        wallSprite = wall.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            wall.isTrigger = true;
            wallSprite.color = Color.gray;
        }
    }
}