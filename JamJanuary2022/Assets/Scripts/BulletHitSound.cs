using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitSound : MonoBehaviour
{
    [SerializeField] GameObject soundPopPrefab;

    private void OnCollisionEnter(Collision other) {
        if (other.transform.tag == "Enemy"){
            GameObject.Instantiate(soundPopPrefab, transform.position, transform.rotation);
        }
    }
}
