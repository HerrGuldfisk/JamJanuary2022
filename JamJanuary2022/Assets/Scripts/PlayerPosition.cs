using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public static Vector3 position;

    void FixedUpdate()
    {
        position = transform.position;
    }
}
