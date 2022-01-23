using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAwayFromTagged : MonoBehaviour
{
    [SerializeField] string tagToRotateFrom = "Player";
    [SerializeField] Vector3 extraRotation = Vector3.zero;

    void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag(tagToRotateFrom).transform, Vector3.up);
        transform.Rotate(extraRotation);
    }
}
