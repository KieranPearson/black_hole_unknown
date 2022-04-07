using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject powerup;

    public static PowerupManager instance { get; private set; }

    private Transform powerupTransform;
    private bool powerupActive;
    private PowerupMovement powerupMovement;

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
        powerupTransform = powerup.transform;
        powerupMovement = powerup.GetComponent<PowerupMovement>();
    }

    private void SpawnPowerup(Vector2 position)
    {
        if (powerup.activeSelf) return;
        Vector3 powerupPosition = powerupTransform.position;
        powerupTransform.position = new Vector3(position.x, position.y, powerupPosition.z);
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
