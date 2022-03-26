using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class AsteroidCollisionHandler : MonoBehaviour
{
    [SerializeField] private Sprite[] asteroidSprites;
    [SerializeField] private int damage = 0;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private int maxDamage;

    void Start()
    {
        maxDamage = asteroidSprites.Length - 1;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            Destroy(collision.gameObject);
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        if (damage == maxDamage)
        {
            gameObject.SetActive(false);
            return;
        }
        damage++;
        UpdateSprite();
        UpdateBoxCollider();
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = asteroidSprites[damage];
    }

    private void UpdateBoxCollider()
    {
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;
        boxCollider.size = spriteSize;
    }
}
