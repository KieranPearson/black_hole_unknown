using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerCollisionHandler : MonoBehaviour
{
    public static event System.Action OnPlayerHit;
    public static event System.Action<Vector2, float> OnImpact;

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
            Vector3 position = transform.position;
            OnImpact?.Invoke(new Vector2(position.x, position.y), 1f);
            colliderObject.SetActive(false);
            OnPlayerHit?.Invoke();
        }
    }
}
