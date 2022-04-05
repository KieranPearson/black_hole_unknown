using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCollisionHandler : MonoBehaviour
{
    public static event System.Action OnPlayerHit;

    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObject = collider.gameObject;
        if (colliderObject.CompareTag("EnemyProjectile"))
        {
            colliderObject.SetActive(false);
            OnPlayerHit?.Invoke();
        }
    }
}
