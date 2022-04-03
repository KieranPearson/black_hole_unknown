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
    private static List<GameObject> activePlayerProjectiles = new List<GameObject>();
    private static List<GameObject> activeEnemyProjectiles = new List<GameObject>();
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

    private int? getProjectileIndexInActivePlayerProjectiles(GameObject projectile)
    {
        for (int i = 0; i < activePlayerProjectiles.Count; i++)
        {
            if (activePlayerProjectiles[i] == projectile) return i;
        }
        return null;
    }

    private int? getProjectileIndexInActiveEnemyProjectiles(GameObject projectile)
    {
        for (int i = 0; i < activeEnemyProjectiles.Count; i++)
        {
            if (activeEnemyProjectiles[i] == projectile) return i;
        }
        return null;
    }

    private void Combat_OnProjectileFired(GameObject projectile)
    {
        Vector3 projectilePosition = projectile.transform.position;
        if (projectile.CompareTag("PlayerProjectile"))
        {
            if (getProjectileIndexInActivePlayerProjectiles(projectile) != null) return;
            activePlayerProjectiles.Add(projectile);
            activeProfile.AddPlayerProjectilePosition(projectilePosition.x, projectilePosition.y);
        }
        else if (projectile.CompareTag("EnemyProjectile"))
        {
            if (getProjectileIndexInActiveEnemyProjectiles(projectile) != null) return;
            activeEnemyProjectiles.Add(projectile);
            activeProfile.AddEnemyProjectilePosition(projectilePosition.x, projectilePosition.y);
        }
    }

    private void ProjectileRemoved(GameObject projectile)
    {
        if (projectile.CompareTag("PlayerProjectile"))
        {
            int? projectileIndex = getProjectileIndexInActivePlayerProjectiles(projectile);
            if (projectileIndex == null) return;
            activePlayerProjectiles.Remove(projectile);
            activeProfile.RemovePlayerProjectilePosition((int) projectileIndex);
        }
        else if (projectile.CompareTag("EnemyProjectile"))
        {
            int? projectileIndex = getProjectileIndexInActiveEnemyProjectiles(projectile);
            if (projectileIndex == null) return;
            activeEnemyProjectiles.Remove(projectile);
            activeProfile.RemoveEnemyProjectilePosition((int) projectileIndex);
        }
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

    private void LoadPlayerProjectiles()
    {
        List<float[]> playerProjectilePositions = activeProfile.GetPlayerProjectilePositions();
        for (int i = 0; i < playerProjectilePositions.Count; i++)
        {
            float xPosition = playerProjectilePositions[i][0];
            float yPosition = playerProjectilePositions[i][1];
            GameObject projectile = ProjectileLoader.instance.LoadPlayerProjectile(xPosition, yPosition);
            activePlayerProjectiles.Add(projectile);
        }
    }

    private void LoadEnemyProjectiles()
    {
        List<float[]> enemyProjectilePositions = activeProfile.GetEnemyProjectilePositions();
        for (int i = 0; i < enemyProjectilePositions.Count; i++)
        {
            float xPosition = enemyProjectilePositions[i][0];
            float yPosition = enemyProjectilePositions[i][1];
            GameObject projectile = ProjectileLoader.instance.LoadEnemyProjectile(xPosition, yPosition);
            activeEnemyProjectiles.Add(projectile);
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
        LoadPlayerProjectiles();
        LoadEnemyProjectiles();
    }

    void Start()
    {
        GenerateEnemyGrid();
        LoadLevel();
    }

    private void FixedUpdate()
    {
        SyncProfileData();
    }

    private void SyncProfileData()
    {
        activeProfile.SetEnemiesXPosition(enemiesTransform.position.x);
        activeProfile.SetEnemiesYPosition(enemiesTransform.position.y);
        activeProfile.SetEnemiesSpeed(enemiesMovement.GetCurrentSpeed());
        activeProfile.SetPlayerXPosition(playerTransform.position.x);
        SyncPlayerProjectilePositions();
        SyncEnemyProjectilePositions();
    }

    private void SyncPlayerProjectilePositions()
    {
        activeProfile.ClearPlayerProjectilePositions();
        for (int i = 0; i < activePlayerProjectiles.Count; i++)
        {
            Vector3 position = activePlayerProjectiles[i].transform.position;
            activeProfile.AddPlayerProjectilePosition(position.x, position.y);
        }
    }

    private void SyncEnemyProjectilePositions()
    {
        activeProfile.ClearEnemyProjectilePositions();
        for (int i = 0; i < activeEnemyProjectiles.Count; i++)
        {
            Vector3 position = activeEnemyProjectiles[i].transform.position;
            activeProfile.AddEnemyProjectilePosition(position.x, position.y);
        }
    }
}
