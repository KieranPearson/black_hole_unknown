using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PowerupTypeChanger : MonoBehaviour
{
    [SerializeField] Sprite rapidfirePowerupSprite;
    [SerializeField] Sprite clonePowerupSprite;
    [SerializeField] Sprite slowMissilesPowerupSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangePowerupToRapidfire()
    {
        spriteRenderer.sprite = rapidfirePowerupSprite;
        gameObject.tag = "RapidfirePowerup";
    }

    public void ChangePowerupToClone()
    {
        spriteRenderer.sprite = clonePowerupSprite;
        gameObject.tag = "ClonePowerup";
    }

    public void ChangePowerupToSlowMissiles()
    {
        spriteRenderer.sprite = slowMissilesPowerupSprite;
        gameObject.tag = "SlowMissilesPowerup";
    }
}
