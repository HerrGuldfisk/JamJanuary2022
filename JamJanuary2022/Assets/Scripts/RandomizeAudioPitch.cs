using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAudioPitch : MonoBehaviour
{
    [SerializeField] float pitchDivergence = 0;

    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch += Random.Range(-pitchDivergence, pitchDivergence);
       
    }
}
