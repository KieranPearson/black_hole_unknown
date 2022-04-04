using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerSpriteUpdater : MonoBehaviour
{
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite strafeLeftSprite;
    [SerializeField] private Sprite strafeRightSprite;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetIdleSprite()
    {
        spriteRenderer.sprite = idleSprite;
    }

    public void SetStrafeLeftSprite()
    {
        spriteRenderer.sprite = strafeLeftSprite;
    }

    public void SetStrafeRightSprite()
    {
        spriteRenderer.sprite = strafeRightSprite;
    }
}
