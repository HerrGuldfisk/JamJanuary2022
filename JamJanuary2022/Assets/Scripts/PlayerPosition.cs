using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    public static Vector3 position;

    private void Awake()
    {
        position = transform.position;
    }

    void FixedUpdate()
    {
        position = transform.position;
    }
}
