using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject powerup;

    private Transform powerupTransform;
    private bool powerupActive;
    private PowerupMovement powerupMovement;

    private void Awake()
    {
        powerupTransform = powerup.transform;
        powerupMovement = powerup.GetComponent<PowerupMovement>();
    }

    private void SpawnPowerup(Vector2 position)
    {
        if (powerup.activeSelf) return;
        powerupTransform.position = position;
        powerup.SetActive(true);
        powerupMovement.MoveUp();
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
