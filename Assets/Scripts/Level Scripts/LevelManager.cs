using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemiesObject;
    [SerializeField] private int enemyRows;
    [SerializeField] private int enemyColumns;
    [SerializeField] private float enemyRowPadding;
    [SerializeField] private float enemyColumnPadding;
    [SerializeField] private GameObject playerObject;

    private static GameObject[,] enemies;
    private static List<List<GameObject>> aliveEnemies = new List<List<GameObject>>();
    private static int enemiesRemaining;
    private static Profile activeProfile;
    private static List<GameObject> activeProjectiles = new List<GameObject>();
    private Transform enemiesTransform;
    private EnemiesMovement enemiesMovement;
    private Transform playerTransform;

    private void Awake()
    {
        enemiesMovement = enemiesObject.GetComponent<EnemiesMovement>();
        enemiesTransform = enemiesObject.transform;
        playerTransform = playerObject.transform;
    }

    public static List<List<GameObject>> GetAliveEnemies()
    {
        return aliveEnemies;
    }

    private Vector2Int? GetColumnRowOfEnemy(GameObject enemy)
    {
        for (int column = 0; column < enemies.GetLength(0); column++)
        {
            for (int row = 0; row < enemies.GetLength(1); row++)
            {
                if (enemies[column, row] != enemy) continue;
                return new Vector2Int(column, row);
            }
        }
        return null;
    }

    private Vector2Int? GetColumnRowOfAliveEnemy(GameObject enemy)
    {
        for (int column = 0; column < aliveEnemies.Count; column++)
        {
            for (int i = 0; i < aliveEnemies[column].Count; i++)
            {
                if (aliveEnemies[column][i] != enemy) continue;
                return new Vector2Int(column, i);
            }
        }
        return null;
    }

    private void SetEnemyDestroyed(int column, int row)
    {
        enemiesRemaining--;
        aliveEnemies[column].RemoveAt(row);
        if (aliveEnemies[column].Count == 0)
        {
            aliveEnemies.RemoveAt(column);
        }
    }

    private void SyncEnemyDeath(GameObject enemy)
    {
        Vector2Int? enemyColumnRow = GetColumnRowOfEnemy(enemy);
        if (enemyColumnRow == null) return;
        int column = enemyColumnRow.Value.x;
        int row = enemyColumnRow.Value.y;
        activeProfile.AddDestroyedEnemey(column, row);
    }

    private void EnemyDestroyed(GameObject enemy, bool isLoaded)
    {
        Vector2Int? enemyAliveColumnRow = GetColumnRowOfAliveEnemy(enemy);
        int column = enemyAliveColumnRow.Value.x;
        int row = enemyAliveColumnRow.Value.y;
        if (enemyAliveColumnRow == null) return;
        SetEnemyDestroyed(column, row);

        if (isLoaded) return;
        SyncEnemyDeath(enemy);
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        EnemyDestroyed(enemy, false);
    }

    private GameObject CreateEnemy(Vector2 position)
    {
        GameObject enemy = Instantiate(enemyPrefab);
        enemy.transform.parent = enemiesTransform;
        Vector2 newPosition = position;
        enemy.transform.position = newPosition;
        return enemy;
    }
    
    private void GenerateEnemyGrid()
    {
        enemies = new GameObject[enemyColumns, enemyRows];
        int columnLength = enemies.GetLength(0);
        int rowLength = enemies.GetLength(1);
        int columnLength_ = columnLength - 1;
        int rowLength_ = rowLength - 1;

        float spawnPosX = -(columnLength_ * (enemyColumnPadding)) / 2f;
        for (int column = 0; column < columnLength; column++)
        {
            List<GameObject> enemiesInColumn = new List<GameObject>();
            float spawnPosY = -(rowLength_ * (enemyRowPadding)) / 2f;
            for (int row = 0; row < rowLength; row++)
            {
                GameObject enemy = CreateEnemy(new Vector2(spawnPosX, spawnPosY));
                enemies[column, row] = enemy;
                enemiesInColumn.Add(enemy);
                spawnPosY += enemyRowPadding;
                enemiesRemaining++;
            }
            aliveEnemies.Add(enemiesInColumn);
            spawnPosX += enemyColumnPadding;
        }
    }

    private bool isProjectileInActiveProjectiles(GameObject projectile)
    {
        for (int i = 0; i < activeProjectiles.Count; i++)
        {
            if (activeProjectiles[i] == projectile) return true;
        }
        return false;
    }

    private void Combat_OnProjectileFired(GameObject projectile)
    {
        if (isProjectileInActiveProjectiles(projectile)) return;
        activeProjectiles.Add(projectile);
    }

    private void ProjectileRemoved(GameObject projectile)
    {
        if (!isProjectileInActiveProjectiles(projectile)) return;
        activeProjectiles.Remove(projectile);
    }

    private void DisableOffscreen_OnProjectileRemoved(GameObject projectile)
    {
        ProjectileRemoved(projectile);
    }

    private void AsteroidCollisionHandler_OnProjectileRemoved(GameObject projectile)
    {
        ProjectileRemoved(projectile);
    }

    private void EnemyCollisionHandler_OnProjectileRemoved(GameObject projectile)
    {
        ProjectileRemoved(projectile);
    }

    void OnEnable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
        Combat.OnProjectileFired += Combat_OnProjectileFired;
        DisableOffscreen.OnProjectileRemoved += DisableOffscreen_OnProjectileRemoved;
        AsteroidCollisionHandler.OnProjectileRemoved += AsteroidCollisionHandler_OnProjectileRemoved;
        EnemyCollisionHandler.OnProjectileRemoved += EnemyCollisionHandler_OnProjectileRemoved;
    }

    void OnDisable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
        Combat.OnProjectileFired -= Combat_OnProjectileFired;
        DisableOffscreen.OnProjectileRemoved -= DisableOffscreen_OnProjectileRemoved;
        AsteroidCollisionHandler.OnProjectileRemoved -= AsteroidCollisionHandler_OnProjectileRemoved;
        EnemyCollisionHandler.OnProjectileRemoved -= EnemyCollisionHandler_OnProjectileRemoved;
    }

    private void LoadDestroyedEnemies()
    {
        List<int[]> destroyedEnemies = activeProfile.GetDestroyedEnemies();
        for (int i = 0; i < destroyedEnemies.Count; i++)
        {
            int column = destroyedEnemies[i][0];
            int row = destroyedEnemies[i][1];
            GameObject enemy = enemies[column, row];
            enemy.SetActive(false);
            EnemyDestroyed(enemy, true);
        }
    }

    private void LoadLevel()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();

        float enemiesXPosition = activeProfile.GetEnemiesXPosition();
        float enemiesYPosition = activeProfile.GetEnemiesYPosition();
        float enemiesSpeed = activeProfile.GetEnemiesSpeed();
        enemiesTransform.position = new Vector3(enemiesXPosition, enemiesYPosition, 0);
        enemiesMovement.SetCurrentSpeed(enemiesSpeed);

        float playerPositionX = activeProfile.GetPlayerXPosition();
        float playerPositionY = playerTransform.position.y;
        float playerPositionZ = playerTransform.position.z;
        playerTransform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        LoadDestroyedEnemies();
    }

    void Start()
    {
        GenerateEnemyGrid();
        LoadLevel();
    }

    private void FixedUpdate()
    {
        SyncProfileSessionData();
    }

    private void SyncProfileSessionData()
    {
        activeProfile.SetEnemiesXPosition(enemiesTransform.position.x);
        activeProfile.SetEnemiesYPosition(enemiesTransform.position.y);
        activeProfile.SetEnemiesSpeed(enemiesMovement.GetCurrentSpeed());
        activeProfile.SetPlayerXPosition(playerTransform.position.x);
    }
}
