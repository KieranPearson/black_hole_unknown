using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLoader : MonoBehaviour
{
    [SerializeField] private GameObject playerProjectilePrefab;
    [SerializeField] private GameObject enemyProjectilePrefab;

    [SerializeField] private Combat playerCombat;
    [SerializeField] private Combat enemyCombat;

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
        return LoadProjectile(enemyProjectilePrefab, x, y, speed, movesDown);
    }
}
