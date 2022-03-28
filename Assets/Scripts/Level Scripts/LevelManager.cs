using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform enemiesTransform;
    [SerializeField] private int enemyRows;
    [SerializeField] private int enemyColumns;
    [SerializeField] private float enemyRowPadding;
    [SerializeField] private float enemyColumnPadding;

    private static GameObject[,] enemies;
    private static List<List<GameObject>> aliveEnemies = new List<List<GameObject>>();
    private static int enemiesRemaining;

    public static List<List<GameObject>> GetAliveEnemies()
    {
        return aliveEnemies;
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        enemiesRemaining--;
        for (int column = 0; column < aliveEnemies.Count; column++)
        {
            for (int i = 0; i < aliveEnemies[column].Count; i++)
            {
                if (aliveEnemies[column][i] != enemy) continue;
                aliveEnemies[column].RemoveAt(i);
                if (aliveEnemies[column].Count == 0)
                {
                    aliveEnemies.RemoveAt(column);
                }
                break;
            }
        }
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

    void OnEnable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
    }

    void OnDisable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
    }

    void Start()
    {
        GenerateEnemyGrid();
        enemiesTransform.position = new Vector3(0, 5, 0);
    }
}
