using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionHandler : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int explosionPoolSize;

    private GameObject[] explosionPool;
    private int currentExplosion;

    private void SetupExplosionPool()
    {
        explosionPool = new GameObject[explosionPoolSize];
        for (int i = 0; i < explosionPoolSize; i++)
        {
            GameObject newExplosion = Instantiate(explosionPrefab);
            newExplosion.SetActive(false);
            newExplosion.name = explosionPrefab.name;
            newExplosion.transform.parent = transform;
            explosionPool[i] = newExplosion;
        }
    }

    private void Start()
    {
        SetupExplosionPool();
    }

    private void DisplayExplosion(Vector2 position, float size)
    {
        GameObject explosion = explosionPool[currentExplosion];
        Transform explosionTransform = explosion.transform;
        Vector3 explosionPosition = explosionTransform.position;
        Vector3 newPosition = new Vector3(position.x, position.y, explosionPosition.z);
        explosionTransform.position = newPosition;
        explosionTransform.localScale = new Vector3(size, size, explosionTransform.localScale.z);
        explosion.SetActive(true);
        currentExplosion++;
        if (currentExplosion >= explosionPoolSize)
        {
            currentExplosion = 0;
        }
    }

    void OnEnable()
    {
        AsteroidCollisionHandler.OnImpact += DisplayExplosion;
        ProjectileCollisionHandler.OnImpact += DisplayExplosion;
        EnemyCollisionHandler.OnImpact += DisplayExplosion;
        PlayerCollisionHandler.OnImpact += DisplayExplosion;
        PowerupCollisionHandler.OnImpact += DisplayExplosion;
    }

    void OnDisable()
    {
        AsteroidCollisionHandler.OnImpact -= DisplayExplosion;
        ProjectileCollisionHandler.OnImpact -= DisplayExplosion;
        EnemyCollisionHandler.OnImpact -= DisplayExplosion;
        PlayerCollisionHandler.OnImpact -= DisplayExplosion;
        PowerupCollisionHandler.OnImpact -= DisplayExplosion;
    }
}
