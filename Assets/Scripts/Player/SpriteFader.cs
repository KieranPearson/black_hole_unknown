using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFader : MonoBehaviour
{
    [SerializeField] private bool fadeIn;
    [SerializeField] private float fadeSpeed;

    private SpriteRenderer spriteRenderer;
    private Color spriteColor;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a);
    }

    private void UpdateSpriteAlpha(float alpha)
    {
        spriteColor.a = alpha;
        spriteRenderer.color = spriteColor;
    }

    private void SetupFade()
    {
        if (fadeIn)
        {
            UpdateSpriteAlpha(0f);
            return;
        }
        UpdateSpriteAlpha(1f);
    }

    private void OnEnable()
    {
        SetupFade();
    }

    private void FadeOut()
    {
        if (spriteColor.a > 0)
        {
            UpdateSpriteAlpha(spriteColor.a -= (Time.deltaTime * fadeSpeed));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void FadeIn()
    {
        if (spriteColor.a < 1)
        {
            UpdateSpriteAlpha(spriteColor.a += (Time.deltaTime * fadeSpeed));
        }
        else
        {
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
