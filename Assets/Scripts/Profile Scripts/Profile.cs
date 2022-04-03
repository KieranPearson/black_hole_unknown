using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Profile
{
    private string name;
    private int highscore;


    private int currentScore;

    private List<int[]> destroyedEnemies; // {column, row}
    // {x, y}
    private List<float[]> playerProjectilePositions;
    private List<float[]> enemyProjectilePositions;

    private float enemiesXPosition;
    private float enemiesYPosition;
    private float enemiesSpeed;

    private float playerXPosition;

    public Profile(string name)
    {
        this.name = name;
        highscore = 0;
        destroyedEnemies = new List<int[]>();
        playerProjectilePositions = new List<float[]>();
        enemyProjectilePositions = new List<float[]>();
        enemiesXPosition = 0;
        enemiesYPosition = 5;
        enemiesSpeed = 0.5f;
        playerXPosition = 0;
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
}
