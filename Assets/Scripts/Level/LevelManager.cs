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
    [SerializeField] private Transform projectiles;
    [SerializeField] private Asteroids asteroids;

    public static event System.Action OnLevelLoaded;
    public static event System.Action OnNewLevelStarted;
    private static GameObject[,] enemies;
    private static List<List<GameObject>> allEnemies = new List<List<GameObject>>();
    private static List<List<GameObject>> aliveEnemies = new List<List<GameObject>>();
    private static int enemiesRemaining;
    private static Profile activeProfile;
    private Transform enemiesTransform;
    private EnemiesMovement enemiesMovement;
    private Transform playerTransform;
    private int totalEnemies;

    private void Awake()
    {
        enemiesMovement = enemiesObject.GetComponent<EnemiesMovement>();
        enemiesTransform = enemiesObject.transform;
        playerTransform = playerObject.transform;
        totalEnemies = enemyRows * enemyColumns;
        enemiesRemaining = totalEnemies;
    }

    private void ClearAllProjectiles()
    {
        for (int i = 0; i < projectiles.childCount; i++)
        {
            GameObject projectile = projectiles.GetChild(i).gameObject;
            projectile.SetActive(false);
        }
    }

    private void RefreshEnemies()
    {
        enemiesObject.SetActive(false);
        aliveEnemies.Clear();
        enemiesRemaining = totalEnemies;
        for (int column = 0; column < enemyColumns; column++)
        {
            List<GameObject> enemiesInColumn = new List<GameObject>();
            for (int row = 0; row < enemyRows; row++)
            {
                GameObject enemy = enemies[column, row];
                enemiesInColumn.Add(enemy);
                enemy.SetActive(true);
            }
            aliveEnemies.Add(enemiesInColumn);
        }
        Vector3 enemiesPosition = enemiesTransform.position;
        float newX = activeProfile.GetEnemiesXPosition();
        float newY = activeProfile.GetEnemiesYPosition();
        enemiesTransform.position = new Vector3(newX, newY, enemiesPosition.z);
        enemiesObject.SetActive(true);
    }

    private void RefreshPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        float newX = activeProfile.GetPlayerXPosition();
        playerTransform.position = new Vector3(newX, playerPosition.y, playerPosition.z);
    }

    private void RefreshAsteroids()
    {
        Transform[] asteroidClusters = asteroids.GetAsteroidClusters();
        for (int clusterIndex = 0; clusterIndex < asteroidClusters.Length; clusterIndex++)
        {
            AsteroidCluster asteroidCluster = asteroidClusters[clusterIndex].GetComponent<AsteroidCluster>();

            GameObject[] asteroidsInCluster = asteroidCluster.GetAsteroids();
            for (int asteroidIndex = 0; asteroidIndex < asteroidsInCluster.Length; asteroidIndex++)
            {
                GameObject asteroid = asteroidsInCluster[asteroidIndex];
                asteroid.SetActive(true);
                AsteroidCollisionHandler asteroidCollision = asteroid.GetComponent<AsteroidCollisionHandler>();
                asteroidCollision.SetDamage(0);
            }
        }
    }

    private void StartNewLevel()
    {
        activeProfile.ClearLevelSpecificData();
        ClearAllProjectiles();
        RefreshEnemies();
        RefreshPlayer();
        RefreshAsteroids();
        OnNewLevelStarted?.Invoke();
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
        if (enemyAliveColumnRow == null) return;
        int column = enemyAliveColumnRow.Value.x;
        int row = enemyAliveColumnRow.Value.y;
        SetEnemyDestroyed(column, row);
        enemiesRemaining--;
        if (isLoaded) return;
        SyncEnemyDeath(enemy);
        enemy.SetActive(false);
        if (enemiesRemaining <= 0)
        {
            StartNewLevel();
        }
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
            }
            allEnemies.Add(enemiesInColumn);
            aliveEnemies.Add(enemiesInColumn);
            spawnPosX += enemyColumnPadding;
        }
    }

    void OnEnable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
        GameQuitHandler.OnRequestDataSync += GameQuitHandler_OnRequestDataSync;
    }

    void OnDisable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
        GameQuitHandler.OnRequestDataSync -= GameQuitHandler_OnRequestDataSync;
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
        }
    }

    private void LoadAsteroidDamage()
    {
        List<List<int>> asteroidDamageStates = activeProfile.GetAsteroidDamageStates();
        Transform[] asteroidClusters = asteroids.GetAsteroidClusters();
        for (int clusterIndex = 0; clusterIndex < asteroidDamageStates.Count; clusterIndex++)
        {
            AsteroidCluster asteroidCluster = asteroidClusters[clusterIndex].GetComponent<AsteroidCluster>();
            GameObject[] asteroidsInCluster = asteroidCluster.GetAsteroids();
            int clusterSize = asteroidDamageStates[clusterIndex].Count;
            for (int asteroidIndex = 0; asteroidIndex < clusterSize; asteroidIndex++)
            {
                GameObject asteroid = asteroidsInCluster[asteroidIndex];
                AsteroidCollisionHandler asteroidCollision = asteroid.GetComponent<AsteroidCollisionHandler>();
                int newDamage = asteroidDamageStates[clusterIndex][asteroidIndex];
                asteroidCollision.SetDamage(newDamage);
            }
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
        LoadAsteroidDamage();

        OnLevelLoaded?.Invoke();
    }

    void Start()
    {
        GenerateEnemyGrid();
        LoadLevel();
    }

    private void GameQuitHandler_OnRequestDataSync()
    {
        SyncProfileData();
        GameQuitHandler.DataSynced();
    }

    private void SyncProfileData()
    {
        activeProfile.SetEnemiesXPosition(enemiesTransform.position.x);
        activeProfile.SetEnemiesYPosition(enemiesTransform.position.y);
        activeProfile.SetEnemiesSpeed(enemiesMovement.GetCurrentSpeed());
        activeProfile.SetPlayerXPosition(playerTransform.position.x);
        SyncProjectilePositions();
        SyncAsteroidDamage();
        
    }

    private void SyncProjectilePositions()
    {
        activeProfile.ClearEnemyProjectilePositions();
        activeProfile.ClearPlayerProjectilePositions();
        for (int i = 0; i < projectiles.childCount; i++)
        {
            GameObject projectile = projectiles.GetChild(i).gameObject;
            if (!projectile.activeSelf) continue;
            Vector3 position = projectile.transform.position;
            if (projectile.CompareTag("EnemyProjectile"))
            {
                activeProfile.AddEnemyProjectilePosition(position.x, position.y);
            }
            else if (projectile.CompareTag("PlayerProjectile"))
            {
                activeProfile.AddPlayerProjectilePosition(position.x, position.y);
            }
        }
    }

    private void SyncAsteroidDamage()
    {
        activeProfile.ClearAsteroidDamageStates();

        Transform[] asteroidClusters = asteroids.GetAsteroidClusters();
        for (int clusterIndex = 0; clusterIndex < asteroidClusters.Length; clusterIndex++)
        {
            AsteroidCluster asteroidCluster = asteroidClusters[clusterIndex].GetComponent<AsteroidCluster>();
            activeProfile.AddAsteroidCluster();
            GameObject[] asteroidsInCluster = asteroidCluster.GetAsteroids();
            for (int asteroidIndex = 0; asteroidIndex < asteroidsInCluster.Length; asteroidIndex++)
            {
                GameObject asteroid = asteroidsInCluster[asteroidIndex];
                AsteroidCollisionHandler asteroidCollision = asteroid.GetComponent<AsteroidCollisionHandler>();
                if (!asteroid.activeSelf)
                {
                    activeProfile.AddAsteroidDamage(clusterIndex, asteroidCollision.GetMaxDamage() + 1);
                    continue;
                }
                activeProfile.AddAsteroidDamage(clusterIndex, asteroidCollision.GetDamage());
            }
        }
    }
}
