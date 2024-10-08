using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Profile
{
    private string name;
    private int highscore;
    private int currentScore;
    private int level;
    private int lives;
    private List<int[]> destroyedEnemies;
    private List<float[]> playerProjectilePositions;
    private List<float[]> enemyProjectilePositions;
    private List<List<int>> asteroidDamageStates;
    private List<string> achievements;
    private float enemiesXPosition;
    private float enemiesYPosition;
    private float enemiesSpeed;
    private float playerXPosition;
    private string activePowerup;
    private float powerupXPosition;
    private float powerupYPosition;
    private bool usingPowerup;
    private int powerupRemainingSeconds;
    private string powerupUsed;
    private bool powerupUsedDuringLevel;
    private bool asteroidShotDuringLevel;

    public Profile()
    {
        SetDefaults();
    }

    public Profile(string name)
    {
        this.name = name;
        SetDefaults();
    }

    public void GenerateRandomPlayerXPosition()
    {
        int randomInt = Random.Range(0, 4);
        switch (randomInt)
        {
            case 0:
                playerXPosition = -12f;
                break;
            case 1:
                playerXPosition = -4f;
                break;
            case 2:
                playerXPosition = 4f;
                break;
            case 3:
                playerXPosition = 12f;
                break;
            default:
                playerXPosition = 0f;
                break;
        }
    }

    private void SetDefaults()
    {
        highscore = 0;
        currentScore = 0;
        level = 1;
        lives = 3;
        destroyedEnemies = new List<int[]>();
        playerProjectilePositions = new List<float[]>();
        enemyProjectilePositions = new List<float[]>();
        asteroidDamageStates = new List<List<int>>();
        achievements = new List<string>();
        enemiesXPosition = 0f;
        enemiesYPosition = 5f;
        enemiesSpeed = 0.5f;
        playerXPosition = 0f;
        activePowerup = "None";
        powerupXPosition = 0f;
        powerupYPosition = 0f;
        usingPowerup = false;
        powerupRemainingSeconds = 0;
        powerupUsed = "None";
        powerupUsedDuringLevel = false;
        asteroidShotDuringLevel = false;
    }

    public void ClearLevelSpecificData()
    {
        Profile defaultProfile = new Profile();
        this.destroyedEnemies = defaultProfile.GetDestroyedEnemies();
        this.playerProjectilePositions = defaultProfile.GetPlayerProjectilePositions();
        this.enemyProjectilePositions = defaultProfile.GetEnemyProjectilePositions();
        this.asteroidDamageStates = defaultProfile.GetAsteroidDamageStates();
        this.enemiesXPosition = defaultProfile.GetEnemiesXPosition();
        this.enemiesYPosition = defaultProfile.GetEnemiesYPosition();
        this.enemiesSpeed = defaultProfile.GetEnemiesSpeed();
        GenerateRandomPlayerXPosition();
    }

    public string GetName()
    {
        return name;
    }

    public int GetHighscore()
    {
        return highscore;
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public List<int[]> GetDestroyedEnemies()
    {
        return destroyedEnemies;
    }

    public float GetEnemiesXPosition()
    {
        return enemiesXPosition;
    }

    public float GetEnemiesYPosition()
    {
        return enemiesYPosition;
    }

    public float GetEnemiesSpeed()
    {
        return enemiesSpeed;
    }

    public float GetPlayerXPosition()
    {
        return playerXPosition;
    }

    public List<float[]> GetPlayerProjectilePositions()
    {
        return playerProjectilePositions;
    }

    public List<float[]> GetEnemyProjectilePositions()
    {
        return enemyProjectilePositions;
    }

    public List<List<int>> GetAsteroidDamageStates()
    {
        return asteroidDamageStates;
    }

    public int GetLevel()
    {
        return level;
    }

    public int GetLives()
    {
        return lives;
    }

    public string GetActivePowerup()
    {
        return activePowerup;
    }

    public float GetPowerupXPosition()
    {
        return powerupXPosition;
    }

    public float GetPowerupYPosition()
    {
        return powerupYPosition;
    }

    public bool GetUsingPowerup()
    {
        return usingPowerup;
    }

    public int GetPowerupRemainingSeconds()
    {
        return powerupRemainingSeconds;
    }

    public string GetPowerupUsed()
    {
        return powerupUsed;
    }

    public List<string> GetAchievements()
    {
        return achievements;
    }

    public bool GetPowerupUsedDuringLevel()
    {
        return powerupUsedDuringLevel;
    }

    public bool GetAsteroidShotDuringLevel()
    {
        return asteroidShotDuringLevel;
    }

    public void SetPowerupUsedDuringLevel(bool powerupUsedDuringLevel)
    {
        this.powerupUsedDuringLevel = powerupUsedDuringLevel;
    }

    public void SetAsteroidShotDuringLevel(bool asteroidShotDuringLevel)
    {
        this.asteroidShotDuringLevel = asteroidShotDuringLevel;
    }

    public void SetEnemiesXPosition(float enemiesXPosition)
    {
        this.enemiesXPosition = enemiesXPosition;
    }

    public void SetEnemiesYPosition(float enemiesYPosition)
    {
        this.enemiesYPosition = enemiesYPosition;
    }

    public void SetEnemiesSpeed(float enemiesSpeed)
    {
        this.enemiesSpeed = enemiesSpeed;
    }

    public void SetName(string name)
    {
        this.name = name;
    }

    public void SetHighscore(int highscore)
    {
        this.highscore = highscore;
    }

    public void SetPlayerXPosition(float playerXPosition)
    {
        this.playerXPosition = playerXPosition;
    }

    public void SetLevel(int level)
    {
        this.level = level;
    }

    public void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void SetCurrentScore(int currentScore)
    {
        this.currentScore = currentScore;
    }

    public void AddDestroyedEnemey(int column, int row)
    {
        destroyedEnemies.Add(new int[2] {column, row});
    }

    public void AddPlayerProjectilePosition(float x, float y)
    {
        playerProjectilePositions.Add(new float[2] {x, y});
    }

    public void AddEnemyProjectilePosition(float x, float y)
    {
        enemyProjectilePositions.Add(new float[2] { x, y });
    }

    public void AddAsteroidCluster()
    {

        asteroidDamageStates.Add(new List<int>());
    }

    public void AddAsteroidDamage(int clusterIndex, int damage)
    {
        asteroidDamageStates[clusterIndex].Add(damage);
    }

    public void AddAchievement(string achievementName)
    {
        achievements.Add(achievementName);
    }

    public void RemovePlayerProjectilePosition(int index)
    {
        playerProjectilePositions.RemoveAt(index);
    }

    public void RemoveEnemyProjectilePosition(int index)
    {
        enemyProjectilePositions.RemoveAt(index);
    }

    public void ClearPlayerProjectilePositions()
    {
        playerProjectilePositions.Clear();
    }

    public void ClearEnemyProjectilePositions()
    {
        enemyProjectilePositions.Clear();
    }

    public void ClearAsteroidDamageStates()
    {
        asteroidDamageStates.Clear();
    }

    public void SetActivePowerup(string activePowerup)
    {
        this.activePowerup = activePowerup;
    }

    public void SetPowerupXPosition(float powerupXPosition)
    {
        this.powerupXPosition = powerupXPosition;
    }

    public void SetPowerupYPosition(float powerupYPosition)
    {
        this.powerupYPosition = powerupYPosition;
    }

    public void SetUsingPowerup(bool usingPowerup)
    {
        this.usingPowerup = usingPowerup;
    }

    public void SetPowerupRemainingSeconds(int powerupRemainingSeconds)
    {
        this.powerupRemainingSeconds = powerupRemainingSeconds;
    }

    public void SetPowerupUsed(string powerupUsed)
    {
        this.powerupUsed = powerupUsed;
    }
}
