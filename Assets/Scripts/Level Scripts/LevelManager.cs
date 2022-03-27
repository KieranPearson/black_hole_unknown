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

    private GameObject[,] enemies;

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
        enemies = new GameObject[enemyRows, enemyColumns];
        int rowLength = enemies.GetLength(0);
        int colLength = enemies.GetLength(1);
        int rowLength_ = rowLength - 1;
        int colLength_ = colLength - 1;

        float spawnPosY = -(rowLength_ * (enemyRowPadding)) / 2f;
        for (int row = 0; row < rowLength; row++)
        {
            float spawnPosX = -(colLength_ * (enemyColumnPadding)) / 2f;
            for (int column = 0; column < colLength; column++)
            {
                GameObject enemy = CreateEnemy(new Vector2(spawnPosX, spawnPosY));
                enemies[row, column] = enemy;
                spawnPosX += enemyColumnPadding;
            }
            spawnPosY += enemyRowPadding;
        }
    }

    void Start()
    {
        GenerateEnemyGrid();
        enemiesTransform.position = new Vector3(0, 5, 0);
    }
}
