using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPlayback : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        float clipLength = 0;
        
        if (audioSource.clip != null){
            clipLength = audioSource.clip.length;
        }

        StartCoroutine(DestroyAfter(clipLength));
    }

    IEnumerator DestroyAfter(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
