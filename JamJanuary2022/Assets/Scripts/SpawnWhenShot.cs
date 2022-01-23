using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWhenShot : MonoBehaviour
{
    [SerializeField] List<GameObject> objectsToSpawn;

    private void OnCollisionEnter(Collision other) {
        if (other.transform.CompareTag("Bullet")){
            foreach(GameObject obj in objectsToSpawn){
                GameObject.Instantiate(obj, transform.position, transform.rotation);
            }
        }
    }
}
