using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupCollisionHandler : MonoBehaviour
{
    public static event System.Action<Vector2, float> OnImpact;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObj = collider.gameObject;
        if (colliderObj.CompareTag("EnemyProjectile"))
        {
            Vector3 colliderPosition = colliderObj.transform.position;
            OnImpact?.Invoke(new Vector2(colliderPosition.x, colliderPosition.y), 0.6f);
            colliderObj.SetActive(false);
        }
        else if (colliderObj.CompareTag("PlayerProjectile"))
        {
            Vector3 colliderPosition = colliderObj.transform.position;
            OnImpact?.Invoke(new Vector2(colliderPosition.x, colliderPosition.y), 0.6f);
            colliderObj.SetActive(false);

            Vector3 position = transform.position;
            OnImpact?.Invoke(new Vector2(position.x, position.y), 1f);
            gameObject.SetActive(false);
        }
    }
}
