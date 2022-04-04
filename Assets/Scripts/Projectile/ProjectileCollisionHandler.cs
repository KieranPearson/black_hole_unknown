using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject colliderObj = collider.gameObject;
        if (colliderObj.CompareTag("PlayerProjectile") || 
            colliderObj.CompareTag("EnemyProjectile"))
        {
            colliderObj.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
