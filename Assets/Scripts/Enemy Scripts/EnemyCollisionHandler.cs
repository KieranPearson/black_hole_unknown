using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCollisionHandler : MonoBehaviour
{
    public static event System.Action<GameObject> OnEnemyDestroyed;
    public static event System.Action<GameObject> OnProjectileRemoved;

    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            OnEnemyDestroyed?.Invoke(gameObject);
            collider.gameObject.SetActive(false);
            gameObject.SetActive(false);
            OnProjectileRemoved?.Invoke(collider.gameObject);
        } else if (collider.gameObject.CompareTag("Asteroid"))
        {
            collider.gameObject.SetActive(false);
        }
    }
}
