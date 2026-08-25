using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioClip mainBGM;
    void Start()
    {
        if (bgmAudioSource != null && mainBGM != null)
        {
            bgmAudioSource.clip = mainBGM;
            bgmAudioSource.loop = true;
            bgmAudioSource.playOnAwake = false;
            bgmAudioSource.Play();
        }
    }
}