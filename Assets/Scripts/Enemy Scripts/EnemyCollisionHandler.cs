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
        if (collider.gameObject.CompareTag("PlayerProjectile"))
        {
            OnEnemyDestroyed?.Invoke(gameObject);
            Destroy(collider.gameObject);
            gameObject.SetActive(false);
        } else if (collider.gameObject.CompareTag("Asteroid"))
        {
            collider.gameObject.SetActive(false);
        }
    }
}
