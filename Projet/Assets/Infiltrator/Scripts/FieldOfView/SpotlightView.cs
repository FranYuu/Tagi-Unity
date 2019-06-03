using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightView : FOV
{
    private bool detected;

    //variable qui définit la portée du cône
    public float lgVisionLight = 5;
    public float heightVisionLight = 10;
    private float angleVisionLight = -1;


    private const float LIGHT_VISION_LG_ADAPT_TO_GAME = 0.62f;
    private const float LIGHT_VISION_HEIGHT_ADAPT_TO_GAME = 2.434f;

    // Use this for initialization
    new void Start()
    {
        detected = false;
        light.transform.localScale = new Vector3(lgVisionLight / LIGHT_VISION_LG_ADAPT_TO_GAME, heightVisionLight / LIGHT_VISION_HEIGHT_ADAPT_TO_GAME, light.transform.localScale.z);
        source = gameObject;
        lgVision = lgVisionLight;
        heightVision = heightVisionLight;
        angleVision = angleVisionLight;
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    protected override void ComportementDetection()
    {
        detected = true;
        player.gameObject.SendMessage("UnderLight", true);
    }

    protected override void PlusAdistance()
    {
        if (detected)
        {
            player.gameObject.SendMessage("UnderLight", false);
            detected = false;
        }
    }

}
