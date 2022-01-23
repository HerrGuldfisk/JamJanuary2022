using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterPlayback : MonoBehaviour
{
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();

        float clipLength = 0;
        
        if (audioSource!=null && audioSource.clip != null){
            clipLength = audioSource.clip.length;
        }

        if (particleSystem != null){
            clipLength = Mathf.Max(clipLength, particleSystem.main.duration);
        }

        StartCoroutine(DestroyAfter(clipLength));
    }

    IEnumerator DestroyAfter(float delay){
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
