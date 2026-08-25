using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip homeBGM;
    void Start()
    {
        if (bgmAudioSource != null && homeBGM != null)
        {
            bgmAudioSource.clip = homeBGM;
            bgmAudioSource.loop = true;
            bgmAudioSource.playOnAwake = false;
            bgmAudioSource.Play();
        }
    }
}