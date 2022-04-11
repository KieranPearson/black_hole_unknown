using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EnemyCollisionHandler : MonoBehaviour
{
    public static event System.Action<GameObject> OnEnemyDestroyed;
    public static event System.Action<Vector2, float> OnImpact;

    private BoxCollider2D boxCollider;
    private Transform myTransform;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        myTransform = transform;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObject = collider.gameObject;
        if (colliderObject.CompareTag("PlayerProjectile"))
        {
            Vector3 position = myTransform.position;
            OnImpact?.Invoke(new Vector2(position.x, position.y), 1f);
            OnEnemyDestroyed?.Invoke(gameObject);
            colliderObject.SetActive(false);
        } 
        else if (colliderObject.CompareTag("Asteroid"))
        {
            Vector3 asteroidPosition = colliderObject.transform.position;
            OnImpact?.Invoke(new Vector2(asteroidPosition.x, asteroidPosition.y), 0.6f);
            colliderObject.SetActive(false);
        }
    }
}
