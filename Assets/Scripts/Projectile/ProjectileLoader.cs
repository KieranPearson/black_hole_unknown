using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLoader : MonoBehaviour
{
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;

    public static ProjectileLoader instance { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private GameObject LoadProjectile(GameObject projectilePrefab, float x, float y)
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.name = projectilePrefab.name;
        Transform projectileTransform = projectile.transform;
        Vector3 projectilePosition = projectileTransform.position;
        projectile.transform.parent = transform;
        projectileTransform.position = new Vector3(x, y, projectilePosition.z);
        projectile.SetActive(true);
        return projectile;
    }

    public GameObject LoadPlayerProjectile(float x, float y)
    {
        return LoadProjectile(playerProjectilePrefab, x, y);
    }

    public GameObject LoadEnemyProjectile(float x, float y)
    {
        return LoadProjectile(enemyProjectilePrefab, x, y);
    }
}
