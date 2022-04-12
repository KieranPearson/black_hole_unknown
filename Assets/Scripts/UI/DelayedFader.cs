using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DelayedFader : MonoBehaviour
{
    [SerializeField] private float fadeSpeed;
    [SerializeField] private int secondsVisible;

    private CanvasGroup canvasGroup;
    private bool fadingOut;
    private bool waiting;
    private int secondsRemaining;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        canvasGroup.alpha = 0f;
        fadingOut = false;
        waiting = false;
        secondsRemaining = 0;
    }

    IEnumerator TickTime()
    {
        while (secondsRemaining > 0)
        {
            secondsRemaining--;
            yield return new WaitForSeconds(1f);
        }
        waiting = false;
        yield return null;
    }

    private void Update()
    {
        Fade();
    }

    private void Fade()
    {
        if (waiting) return;
        else if (fadingOut)
        {
            FadeOut();
        }
        else
        {
            FadeIn();
        }
    }

    private void FadeIn()
    {
        if (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += (Time.deltaTime * fadeSpeed);
        }
        else
        {
            secondsRemaining = secondsVisible;
            waiting = true;
            fadingOut = true;
            StartCoroutine(TickTime());
        }
    }

    private void FadeOut()
    {
        if (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= (Time.deltaTime * fadeSpeed);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void TickTimeRemaining()
    {

    }
}
