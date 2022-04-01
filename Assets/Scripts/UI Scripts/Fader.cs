using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    [SerializeField] private bool fadeIn;
    [SerializeField] private float fadeSpeed;

    private CanvasGroup canvasGroup;

    public static event System.Action OnScreenFadedIn;
    public static event System.Action OnScreenFadedOut;

    public void ToggleFadeIn()
    {
        fadeIn = !fadeIn;
    }

    private void SetupFade()
    {
        if (fadeIn)
        {
            canvasGroup.alpha = 1f;
            return;
        }
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0f;
    }

    private void OnEnable()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        SetupFade();
    }

    private void FadeIn()
    {
        if (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= (Time.deltaTime * fadeSpeed);
        }
        else
        {
            OnScreenFadedIn?.Invoke();
            gameObject.SetActive(false);
        }
    }

    private void FadeOut()
    {
        if (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += (Time.deltaTime * fadeSpeed);
        }
        else
        {
            OnScreenFadedOut?.Invoke();
            this.enabled = false;
        }
    }

    private void Fade()
    {
        if (fadeIn)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    private void Update()
    {
        Fade();
    }
}
