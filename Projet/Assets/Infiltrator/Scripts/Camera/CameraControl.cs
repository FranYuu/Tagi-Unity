using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public List<GameObject> positions;
    public int i;
    // Use this for initialization
    void Start()
    {
        i = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void moveForward()
    {
        if (i < positions.Count - 1)
        {
            i++;
            transform.position = new Vector3(positions[i].transform.position.x, positions[i].transform.position.y, transform.position.z);
        }
    }
    
    public void moveBackward()
    {
        if (i > 0)
        {
            i--;
            transform.position = new Vector3(positions[i].transform.position.x, positions[i].transform.position.y, transform.position.z);
        }
    }
    public void moveTo(int i)
    {
        if (i < positions.Count - 1)
        {
            transform.position = new Vector3(positions[i].transform.position.x, positions[i].transform.position.y, transform.position.z);
        }
    }

}
