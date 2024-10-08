using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class AsteroidCollisionHandler : MonoBehaviour
{
    [SerializeField] private Sprite[] asteroidSprites;
    [SerializeField] private int damage = 0;

    public static event System.Action<Vector2, float> OnImpact;
    public static event System.Action OnPlayerShotAsteroid;

    private static bool isInvincible;

    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private int maxDamage;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        maxDamage = asteroidSprites.Length - 1;
        isInvincible = false;
    }

    private void ProjectileHit(GameObject projectile)
    {
        Vector3 position = transform.position;
        OnImpact?.Invoke(new Vector2(position.x, position.y), 0.6f);
        projectile.SetActive(false);
        TakeDamage();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObj = collider.gameObject;
        if (colliderObj.CompareTag("PlayerProjectile"))
        {
            OnPlayerShotAsteroid?.Invoke();
            ProjectileHit(collider.gameObject);
        } else if (colliderObj.CompareTag("EnemyProjectile"))
        {
            ProjectileHit(collider.gameObject);
        }
    }

    private void UpdateAsteroidDamage()
    {
        UpdateSprite();
        UpdateBoxCollider();
    }

    public static void ToggleInvincibility(bool toggle)
    {
        isInvincible = toggle;
    }

    private void TakeDamage()
    {
        if (isInvincible) return;
        if (damage == maxDamage)
        {
            gameObject.SetActive(false);
            return;
        }
        damage++;
        UpdateAsteroidDamage();
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

    public int GetMaxDamage()
    {
        return maxDamage;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
        if (damage > maxDamage)
        {
            damage = maxDamage;
            gameObject.SetActive(false);
            return;
        }
        UpdateAsteroidDamage();
    }
}
