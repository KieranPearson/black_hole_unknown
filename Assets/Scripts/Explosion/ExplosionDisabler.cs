using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ExplosionDisabler : MonoBehaviour
{
    private bool ExplosionAnimationFinished;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator DeleteExplosionOnReady()
    {
        while (!ExplosionAnimationFinished || audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.SetActive(false);
        yield return null;
    }

    private void OnEnable()
    {
        ExplosionAnimationFinished = false;
        StartCoroutine(DeleteExplosionOnReady());
    }

    public void OnExplosionAnimationFinished()
    {
        ExplosionAnimationFinished = true;
    }
}
