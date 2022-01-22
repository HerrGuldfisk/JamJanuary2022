using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhenShot : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Bullet")){
            GameObject.Instantiate(objectToSpawn, transform.position, transform.rotation);
        }
    }
}
