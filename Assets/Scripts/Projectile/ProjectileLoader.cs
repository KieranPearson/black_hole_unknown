using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLoader : MonoBehaviour
{
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;

    [SerializeField] private Combat playerCombat;
    [SerializeField] private Combat enemyCombat;

    private List<GameObject> loadedEnemyProjectiles = new List<GameObject>();
    private List<ProjectileMovement> enemyProjectileMovements = new List<ProjectileMovement>();

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

    private GameObject LoadProjectile(GameObject projectilePrefab, float x, float y, 
        float speed, bool movesDown)
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.name = projectilePrefab.name;
        Transform projectileTransform = projectile.transform;
        Vector3 projectilePosition = projectileTransform.position;
        projectile.transform.parent = transform;
        projectileTransform.position = new Vector3(x, y, projectilePosition.z);
        projectile.SetActive(true);
        ProjectileMovement projectileMovement = projectile.GetComponent<ProjectileMovement>();
        if (movesDown) speed = -speed;
        projectileMovement.SetSpeed(speed);
        return projectile;
    }

    public GameObject LoadPlayerProjectile(float x, float y)
    {
        float speed = playerCombat.GetProjectilesDefaultSpeed();
        bool movesDown = playerCombat.ProjectilesDefaultMoveDown();
        return LoadProjectile(playerProjectilePrefab, x, y, speed, movesDown);
    }

    public GameObject LoadEnemyProjectile(float x, float y)
    {
        float speed = enemyCombat.GetProjectilesDefaultSpeed();
        bool movesDown = enemyCombat.ProjectilesDefaultMoveDown();
        GameObject loadedEnemyProjectile = LoadProjectile(enemyProjectilePrefab, x, y, speed, movesDown);
        loadedEnemyProjectiles.Add(loadedEnemyProjectile);
        enemyProjectileMovements.Add(loadedEnemyProjectile.GetComponent<ProjectileMovement>());
        return loadedEnemyProjectile;
    }

    private void RefreshLoadedProjectiles()
    {
        if (loadedEnemyProjectiles.Count <= 0) return;
        for (int i = 0; i < loadedEnemyProjectiles.Count; i++)
        {
            if (loadedEnemyProjectiles[i].activeSelf) continue;
            Destroy(loadedEnemyProjectiles[i]);
            loadedEnemyProjectiles.RemoveAt(i);
            enemyProjectileMovements.RemoveAt(i);
        }
    }

    public List<ProjectileMovement> GetEnemyProjectileMovements()
    {
        RefreshLoadedProjectiles();
        return enemyProjectileMovements;
    }
}
