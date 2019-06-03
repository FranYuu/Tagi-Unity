using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockLevel : MonoBehaviour
{
    public Button Level1;
    // Start is called before the first frame update
    void Start()
    {
        int level = LevelFile.load();

        switch (level) // si on a plusieurs niveau case pour chaque niveau
        {
            case 1:
                Level1.image.color = Color.white;
                Level1.interactable = true;
                break;
        }
    }
}