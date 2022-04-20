using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BackgroundMusic : MonoBehaviour
{
    public static BackgroundMusic instance { get; private set; }

    private AudioSource backgroundMusic;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        backgroundMusic = GetComponent<AudioSource>();
    }

    public void UpdateMusicVolume(float volume)
    {
        backgroundMusic.volume = volume;
    }
}
