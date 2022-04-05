using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCollisionHandler : MonoBehaviour
{
    public static event System.Action<GameObject> OnEnemyDestroyed;

    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObject = collider.gameObject;
        if (colliderObject.CompareTag("PlayerProjectile"))
        {
            OnEnemyDestroyed?.Invoke(gameObject);
            colliderObject.SetActive(false);
        } else if (colliderObject.CompareTag("Asteroid"))
        {
            colliderObject.SetActive(false);
        }
    }
}
