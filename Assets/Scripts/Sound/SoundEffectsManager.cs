using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectsManager : MonoBehaviour
{
    public static SoundEffectsManager instance { get; private set; }

    private float soundEffectsVolume;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        soundEffectsVolume = SettingsManager.instance.GetSoundEffectsVolume();
    }

    private void UpdateSourceVolume(AudioSource source)
    {
        if (source.volume == soundEffectsVolume) return;
        source.volume = soundEffectsVolume;
    }

    private void PlaySound(AudioSource source)
    {
        UpdateSourceVolume(source);
        source.Play();
    }

    private void PlayRandomSound(AudioClip[] sounds, AudioSource source)
    {
        int randomSoundIndex = Random.Range(0, sounds.Length);
        source.clip = sounds[randomSoundIndex];
        UpdateSourceVolume(source);
        source.Play();
    }

    private void OnEnable()
    {
        ExplosionSound.OnPlayExplosionSound += PlayRandomSound;
        Combat.OnPlayFireSound += PlaySound;
        PowerupManager.OnPlayCollectedSound += PlaySound;
        AchievementManager.OnPlayAchievementSound += PlaySound;
    }

    private void OnDisable()
    {
        ExplosionSound.OnPlayExplosionSound -= PlayRandomSound;
        Combat.OnPlayFireSound -= PlaySound;
        PowerupManager.OnPlayCollectedSound -= PlaySound;
        AchievementManager.OnPlayAchievementSound -= PlaySound;
    }
}
