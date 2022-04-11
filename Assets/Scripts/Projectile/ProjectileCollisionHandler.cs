using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionHandler : MonoBehaviour
{
    public static event System.Action<Vector2, float> OnImpact;

    private Transform thisTransform;

    private void Awake()
    {
        thisTransform = transform;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObj = collider.gameObject;
        if (colliderObj.CompareTag("PlayerProjectile") || 
            colliderObj.CompareTag("EnemyProjectile"))
        {
            Vector3 position = thisTransform.position;
            Vector3 colliderPosition = colliderObj.transform.position;
            OnImpact?.Invoke(new Vector2(position.x, position.y), 0.6f);
            OnImpact?.Invoke(new Vector2(colliderPosition.x, colliderPosition.y), 0.6f);

            colliderObj.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
