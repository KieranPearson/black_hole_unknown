using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExplosionSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] explosionSounds;

    public static event System.Action<AudioClip[], AudioSource> OnPlayExplosionSound;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OnPlayExplosionSound?.Invoke(explosionSounds, audioSource);
    }
}
