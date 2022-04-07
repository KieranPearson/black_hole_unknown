using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject powerup;
    [SerializeField] int percentChanceOfPowerup;

    public static PowerupManager instance { get; private set; }

    private Transform powerupTransform;
    private PowerupMovement powerupMovement;
    private PowerupTypeChanger powerupTypeChanger;
    private enum PowerupType {
        RapidfirePowerup,
        ClonePowerup,
        SlowMissilesPowerup
    };

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
        powerupTypeChanger = powerup.GetComponent<PowerupTypeChanger>();
    }

    private void SetPowerupType(PowerupType powerupType)
    {
        switch (powerupType)
        {
            case PowerupType.RapidfirePowerup:
                powerupTypeChanger.ChangePowerupToRapidfire();
                break;
            case PowerupType.ClonePowerup:
                powerupTypeChanger.ChangePowerupToClone();
                break;
            case PowerupType.SlowMissilesPowerup:
                powerupTypeChanger.ChangePowerupToSlowMissiles();
                break;
            default:
                return;
        }
    }

    private void RandomisePowerup()
    {
        int randomNumber = Random.Range(0, 3);
        switch (randomNumber)
        {
            case 0:
                SetPowerupType(PowerupType.RapidfirePowerup);
                break;
            case 1:
                SetPowerupType(PowerupType.ClonePowerup);
                break;
            case 2:
                SetPowerupType(PowerupType.SlowMissilesPowerup);
                break;
            default:
                return;
        }
    }

    private void SpawnPowerup(Vector2 position)
    {
        if (powerup.activeSelf) return;
        Vector3 powerupPosition = powerupTransform.position;
        powerupTransform.position = new Vector3(position.x, position.y, powerupPosition.z);
        powerup.SetActive(true);
        powerupMovement.MoveUp();
        RandomisePowerup();
        
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        int powerupSpawnChance = Random.Range(1, 101);
        if (powerupSpawnChance > percentChanceOfPowerup) return;
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

    public string GetActivePowerup()
    {
        if (!powerup.activeSelf) return "None";
        return powerup.tag;
    }

    public Vector2 GetPowerupPosition()
    {
        Vector3 powerupPosition = powerupTransform.position;
        return new Vector2(powerupPosition.x, powerupPosition.y);
    }
}
