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

    private void DisplayExplosion(Vector2 position)
    {
        GameObject explosion = explosionPool[currentExplosion];
        explosion.SetActive(false);
        Vector3 explosionPosition = explosion.transform.position;
        Vector3 newPosition = new Vector3(position.x, position.y, explosionPosition.z);
        explosion.transform.position = newPosition;
        explosion.SetActive(true);
        currentExplosion++;
        if (currentExplosion >= explosionPoolSize)
        {
            currentExplosion = 0;
        }
    }

    void OnEnable()
    {
        //AsteroidCollisionHandler.OnImpact += DisplayExplosion;
    }

    void OnDisable()
    {
        //AsteroidCollisionHandler.OnImpact -= DisplayExplosion;
    }
}
