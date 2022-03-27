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

    private void CreateEnemy()
    {

    }
    
    private void GenerateEnemyGrid()
    {
        enemies = new GameObject[enemyRows, enemyColumns];
        int rowLength = enemies.GetLength(0);
        int colLength = enemies.GetLength(1);
        int colLength_ = colLength - 1;
        int rowLength_ = rowLength - 1;

        float spawnPosY = -(rowLength_ * (enemyRowPadding)) / 2f;
        for (int row = 0; row < rowLength; row++)
        {
            float spawnPosX = -(colLength_ * (enemyColumnPadding)) / 2f;
            for (int column = 0; column < colLength; column++)
            {
                GameObject enemy = Instantiate(enemyPrefab);
                enemy.transform.parent = enemiesTransform;
                Vector3 newPosition = new Vector3(spawnPosX, spawnPosY);
                enemy.transform.position = newPosition;
                spawnPosX += enemyColumnPadding;
            }
            spawnPosY += enemyRowPadding;
        }
    }

    void Start()
    {
        GenerateEnemyGrid();
    }
}
