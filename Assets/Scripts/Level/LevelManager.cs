using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemiesObject;
    [SerializeField] private int enemyRows;
    [SerializeField] private int enemyColumns;
    [SerializeField] private float enemyRowPadding;
    [SerializeField] private float enemyColumnPadding;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject player2Object;
    [SerializeField] private Transform projectiles;
    [SerializeField] private Asteroids asteroids;
    [SerializeField] private float enemyEndGameYPosition;
    [SerializeField] private float enemySpeedIncreaseOnDestruction;
    [SerializeField] private float enemySpeedOn3Remaining;
    [SerializeField] private float enemySpeedOn2Remaining;
    [SerializeField] private float enemySpeedOn1Remaining;
    [SerializeField] private float enemiesStartPositionYOnLevel2;
    [SerializeField] private float enemiesStartPositionYOnLevel3;
    [SerializeField] private float enemiesStartPositionYOnLevel4;
    [SerializeField] private float enemiesStartPositionYOnLevel5OrMore;
    [SerializeField] private Transform playerCloneTransform;
    [SerializeField] private Transform player2CloneTransform;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private MultiplayerManager multiplayerManager;
    [SerializeField] private SpriteFader playerSpriteFader;
    [SerializeField] private SpriteFader player2SpriteFader;

    public static LevelManager instance { get; private set; }

    public static event System.Action OnLevelLoaded;
    public static event System.Action OnNewLevelStarted;
    public static event System.Action OnGameReset;
    public static event System.Action<string> OnAchievementUnlocked;

    private GameObject[,] enemies;
    private List<Combat> enemiesCombat = new List<Combat>();
    private List<List<GameObject>> allEnemies = new List<List<GameObject>>();
    private List<List<GameObject>> aliveEnemies = new List<List<GameObject>>();
    private int enemiesRemaining;
    private Profile activeProfile;
    private Transform enemiesTransform;
    private EnemiesMovement enemiesMovement;
    private Transform playerTransform;
    private Transform player2Transform;
    private int totalEnemies;
    private PowerupManager powerupManager;
    private Movement playerMovement;
    private Movement player2Movement;
    private PlayerController playerController;
    private PlayerController player2Controller;
    private Combat playerCombat;
    private Combat player2Combat;

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
        enemiesMovement = enemiesObject.GetComponent<EnemiesMovement>();
        enemiesTransform = enemiesObject.transform;
        playerTransform = playerObject.transform;
        player2Transform = player2Object.transform;
        totalEnemies = enemyRows * enemyColumns;
        enemiesRemaining = totalEnemies;
        powerupManager = PowerupManager.instance;
        playerMovement = playerObject.GetComponent<Movement>();
        playerController = playerObject.GetComponent<PlayerController>();
        playerCombat = playerObject.GetComponent<Combat>();
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

    private void RefreshPlayer2()
    {
        if (!multiplayerManager.isMultiplayerModeEnabled()) return;
        Vector3 player2Position = player2Transform.position;
        float newX = activeProfile.GetPlayerXPosition();
        player2Transform.position = new Vector3(newX, player2Position.y, player2Position.z);
    }

    private void RefreshPlayer()
    {
        Vector3 playerPosition = playerTransform.position;
        float newX = activeProfile.GetPlayerXPosition();
        playerTransform.position = new Vector3(newX, playerPosition.y, playerPosition.z);

        newX = playerTransform.position.x - 1.75f;
        Vector3 playerClonePosition = playerCloneTransform.position;
        playerCloneTransform.position = new Vector3(newX, playerClonePosition.y, playerClonePosition.z);
        player2CloneTransform.position = new Vector3(newX, playerClonePosition.y, playerClonePosition.z);
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

    private void RefreshGame()
    {
        activeProfile.ClearLevelSpecificData();
        ClearAllProjectiles();
        RefreshEnemies();
        UpdateEnemySpeed();
        RefreshPlayer();
        RefreshPlayer2();
        RefreshAsteroids();
        activeProfile.SetAsteroidShotDuringLevel(false);
        activeProfile.SetPowerupUsedDuringLevel(false);
    }

    private void SetNewLevelEnemiesYPosition()
    {
        int currentLevel = activeProfile.GetLevel() + 1;
        float newEnemiesYPosition;
        switch (currentLevel)
        {
            case 2:
                newEnemiesYPosition = enemiesStartPositionYOnLevel2;
                break;
            case 3:
                newEnemiesYPosition = enemiesStartPositionYOnLevel3;
                break;
            case 4:
                newEnemiesYPosition = enemiesStartPositionYOnLevel4;
                break;
            default:
                newEnemiesYPosition = enemiesStartPositionYOnLevel5OrMore;
                break;
        }
        activeProfile.SetEnemiesYPosition(newEnemiesYPosition);
        Vector3 enemiesPosition = enemiesTransform.position;
        enemiesTransform.position = new Vector3(enemiesPosition.x, newEnemiesYPosition, enemiesPosition.z);
    }

    private void UnlockAchievements()
    {
        if (!activeProfile.GetAsteroidShotDuringLevel())
        {
            OnAchievementUnlocked?.Invoke("Asteroidinary");
        }
        if (!activeProfile.GetPowerupUsedDuringLevel())
        {
            OnAchievementUnlocked?.Invoke("Powerups? Who needs 'em");
        }
    }

    private void StartNewLevel()
    {
        UnlockAchievements();
        RefreshGame();
        SetNewLevelEnemiesYPosition();
        OnNewLevelStarted?.Invoke();
    }

    private void DisplayPlayer()
    {
        playerObject.SetActive(true);
        playerSpriteFader.enabled = true;
        if (multiplayerManager.isMultiplayerModeEnabled())
        {
            player2Object.SetActive(true);
            player2SpriteFader.enabled = true;
        }
    }

    IEnumerator WaitForGameReset()
    {
        while (deathScreen.activeSelf)
        {
            yield return new WaitForSeconds(1);
        }
        DisplayPlayer();
        yield return null;
    }

    private void DisablePlayer2()
    {
        if (!multiplayerManager.isMultiplayerModeEnabled()) return;
        if (multiplayerManager.isMultiplayerModeEnabled()) player2Movement.Stop();
        Command stopMovingLeft = new StopMovingLeftCommand(player2Controller);
        Command stopMovingRight = new StopMovingRightCommand(player2Controller);
        player2Controller.UpdateState(stopMovingLeft);
        player2Controller.UpdateState(stopMovingRight);
        player2Combat.ToggleFire(false);
        player2Object.SetActive(false);
    }

    private void DisablePlayer()
    {
        playerMovement.Stop();
        Command stopMovingLeft = new StopMovingLeftCommand(playerController);
        Command stopMovingRight = new StopMovingRightCommand(playerController);
        playerController.UpdateState(stopMovingLeft);
        playerController.UpdateState(stopMovingRight);
        playerCombat.ToggleFire(false);
        playerObject.SetActive(false);
    }

    private void ResetGame()
    {
        DisablePlayer();
        DisablePlayer2();
        activeProfile.SetPowerupRemainingSeconds(0);
        deathScreen.SetActive(true);
        RefreshGame();
        OnGameReset?.Invoke();
        StartCoroutine(WaitForGameReset());
    }

    private void CheckIfEnemiesAtEndGamePosition()
    {
        for (int column = 0; column < aliveEnemies.Count; column++)
        {
            Transform enemyTransform = aliveEnemies[column][0].transform;
            if (enemyTransform.position.y > enemyEndGameYPosition) continue;
            ResetGame();
        }
    }

    public List<List<GameObject>> GetAliveEnemies()
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

    private void UpdateEnemySpeed()
    {
        float enemiesDestroyed = Mathf.Clamp((totalEnemies - enemiesRemaining), 0, totalEnemies);
        float newSpeed = 0.5f;
        switch (enemiesRemaining)
        {
            case 3:
                newSpeed = enemySpeedOn3Remaining;
                break;
            case 2:
                newSpeed = enemySpeedOn2Remaining;
                break;
            case 1:
                newSpeed = enemySpeedOn1Remaining;
                break;
            default:
                newSpeed += (enemiesDestroyed * enemySpeedIncreaseOnDestruction);
                break;
        }
        activeProfile.SetEnemiesSpeed(newSpeed);
        enemiesMovement.SetSpeed(newSpeed);
    }

    private void CheckEnemyBehindFrontRow(int row)
    {
        if (row == 0) return;
        OnAchievementUnlocked?.Invoke("Sharpshooter");
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
        CheckEnemyBehindFrontRow(row);
        UpdateEnemySpeed();
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
                enemiesCombat.Add(enemy.GetComponent<Combat>());
            }
            allEnemies.Add(enemiesInColumn);
            aliveEnemies.Add(enemiesInColumn);
            spawnPosX += enemyColumnPadding;
        }
    }

    private void StatsManager_OnAllLivesLost()
    {
        ResetGame();
    }

    private void EnemiesMovement_OnEnemiesMovedDown()
    {
        CheckIfEnemiesAtEndGamePosition();
    }

    private void OpenMainMenuCommand_OnExitingLevel()
    {
        SyncProfileData();
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }

    private void HandlePlayerShotAsteroid()
    {
        activeProfile.SetAsteroidShotDuringLevel(true);
    }

    private void HandlePowerupPickedUp()
    {
        activeProfile.SetPowerupUsedDuringLevel(true);
    }

    void OnEnable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
        GameQuitHandler.OnRequestDataSync += GameQuitHandler_OnRequestDataSync;
        StatsManager.OnAllLivesLost += StatsManager_OnAllLivesLost;
        EnemiesMovement.OnEnemiesMovedDown += EnemiesMovement_OnEnemiesMovedDown;
        OpenMainMenuCommand.OnExitingLevel += OpenMainMenuCommand_OnExitingLevel;
        AsteroidCollisionHandler.OnPlayerShotAsteroid += HandlePlayerShotAsteroid;
        PowerupCollisionHandler.OnPowerupPickedUp += HandlePowerupPickedUp;
    }

    void OnDisable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
        GameQuitHandler.OnRequestDataSync -= GameQuitHandler_OnRequestDataSync;
        StatsManager.OnAllLivesLost -= StatsManager_OnAllLivesLost;
        EnemiesMovement.OnEnemiesMovedDown -= EnemiesMovement_OnEnemiesMovedDown;
        OpenMainMenuCommand.OnExitingLevel -= OpenMainMenuCommand_OnExitingLevel;
        AsteroidCollisionHandler.OnPlayerShotAsteroid -= HandlePlayerShotAsteroid;
        PowerupCollisionHandler.OnPowerupPickedUp -= HandlePowerupPickedUp;
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
        enemiesMovement.SetStartSpeed(enemiesSpeed);

        float playerPositionX = activeProfile.GetPlayerXPosition();
        float playerPositionY = playerTransform.position.y;
        float playerPositionZ = playerTransform.position.z;
        playerTransform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);
        player2Transform.position = new Vector3(playerPositionX, playerPositionY, playerPositionZ);

        Vector3 playerClonePosition = playerCloneTransform.position;
        float newPlayerCloneXPosition = playerTransform.position.x - 1.75f;
        playerCloneTransform.position = new Vector3(newPlayerCloneXPosition, playerClonePosition.y, playerClonePosition.z);
        player2CloneTransform.position = new Vector3(newPlayerCloneXPosition, playerClonePosition.y, playerClonePosition.z);

        LoadDestroyedEnemies();
        LoadPlayerProjectiles();
        LoadEnemyProjectiles();
        LoadAsteroidDamage();
        powerupManager.LoadPowerup();

        OnLevelLoaded?.Invoke();
    }

    private void SetupMultiplayer()
    {
        multiplayerManager = MultiplayerManager.instance;
        if (!multiplayerManager.isMultiplayerModeEnabled()) return;
        player2Movement = player2Object.GetComponent<Movement>();
        player2Controller = player2Object.GetComponent<PlayerController>();
        player2Combat = player2Object.GetComponent<Combat>();
    }

    void Start()
    {
        SetupMultiplayer();
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
        powerupManager.SyncPowerup();
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

    public List<Combat> GetAllEnemiesCombat()
    {
        return enemiesCombat;
    }
}
