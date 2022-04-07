using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject powerup;

    private Transform powerupTransform;
    private bool powerupActive;

    private void Awake()
    {
        powerupTransform = powerup.transform;
    }

    private void SpawnPowerup(Vector2 position)
    {
        powerup.SetActive(false);
        powerupTransform.position = position;
        powerup.SetActive(true);
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        Vector3 enemyPosition = enemy.transform.position;
        SpawnPowerup(new Vector2(enemyPosition.x, enemyPosition.y));
    }

    void OnEnable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
    }

    void OnDisable()
    {
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
    }
}
