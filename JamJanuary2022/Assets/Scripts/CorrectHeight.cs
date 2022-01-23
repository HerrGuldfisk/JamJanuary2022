using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrectHeight : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + new Vector3(0, 2, 0), Vector3.down, out hit, Mathf.Infinity))
        {
            return;
        }
        else
        {
            RaycastHit aboveHit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1000, 0), Vector3.down, out aboveHit, Mathf.Infinity))
            {
                transform.position = new Vector3(transform.position.x, aboveHit.point.y, transform.position.z);
            }
        }
    }
}
