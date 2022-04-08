using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    [SerializeField] GameObject powerup;
    [SerializeField] int percentChanceOfPowerup;
    [SerializeField] GameObject playerClone;
    [SerializeField] Combat playerCombat;
    [SerializeField] ProjectileMovement enemyProjectileMovement;
    [SerializeField] int powerupDuration;
    [SerializeField] Transform playerTransform;
    [SerializeField] float rapidfireSpeed;
    [SerializeField] float slowMissilesSpeed;

    public static PowerupManager instance { get; private set; }

    private Transform powerupTransform;
    private PowerupMovement powerupMovement;
    private PowerupTypeChanger powerupTypeChanger;
    private enum PowerupType {
        RapidfirePowerup,
        ClonePowerup,
        SlowMissilesPowerup
    };
    private Profile activeProfile;

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

    private void Start()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();
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
        Vector3 powerupPosition = powerupTransform.position;
        powerupTransform.position = new Vector3(position.x, position.y, powerupPosition.z);
        powerup.SetActive(true);
        powerupMovement.MoveUp();
        RandomisePowerup();
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        if (powerup.activeSelf || activeProfile.GetUsingPowerup()) return;
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

    private string GetActivePowerupName()
    {
        if (!powerup.activeSelf) return "None";
        return powerup.tag;
    }

    private Vector2 GetPowerupPosition()
    {
        Vector3 powerupPosition = powerupTransform.position;
        return new Vector2(powerupPosition.x, powerupPosition.y);
    }

    private PowerupType? GetPowerupTypeByName(string powerupName)
    {
        switch (powerupName)
        {
            case "RapidfirePowerup":
                return PowerupType.RapidfirePowerup;
            case "ClonePowerup":
                return PowerupType.ClonePowerup;
            case "SlowMissilesPowerup":
                return PowerupType.SlowMissilesPowerup;
            default:
                return null;
        }
    }

    private void DisplayPowerup(Vector2 position, string powerupName)
    {
        PowerupType? powerupType = GetPowerupTypeByName(powerupName);
        if (powerupType == null) return;
        Vector3 powerupPosition = powerupTransform.position;
        powerupTransform.position = new Vector3(position.x, position.y, powerupPosition.z);
        powerup.SetActive(true);
        SetPowerupType((PowerupType) powerupType);
    }

    public void SyncPowerup()
    {
        string activePowerup = GetActivePowerupName();
        activeProfile.SetActivePowerup(activePowerup);
        if (activePowerup == "None") return;
        Vector2 powerupPosition = GetPowerupPosition();
        activeProfile.SetPowerupXPosition(powerupPosition.x);
        activeProfile.SetPowerupYPosition(powerupPosition.y);
    }

    public void LoadPowerup()
    {
        string activePowerup = activeProfile.GetActivePowerup();
        if (activePowerup == "None") return;
        float powerupXPosition = activeProfile.GetPowerupXPosition();
        float powerupYPosition = activeProfile.GetPowerupYPosition();
        DisplayPowerup(new Vector2(powerupXPosition, powerupYPosition), activePowerup);
    }

    private void RemovePowerupEffects()
    {
        playerCombat.SetDefaultFireRate();
        playerClone.SetActive(false);
        List<Combat> enemiesCombat = LevelManager.instance.GetAllEnemiesCombat();
        for (int i = 0; i < enemiesCombat.Count; i++)
        {
            enemiesCombat[i].SetProjectilesDefaultSpeed();
        }
    }

    private void UseRapidfirePowerup()
    {
        playerCombat.SetFireRate(rapidfireSpeed);
    }

    private void UseClonePowerup()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 playerClonePosition = playerClone.transform.position;
        playerClone.transform.position = new Vector3(playerPosition.x, playerClonePosition.y, playerClonePosition.z);
        playerClone.SetActive(true);
    }

    private void UseSlowMissilesPowerup()
    {
        List<Combat> enemiesCombat = LevelManager.instance.GetAllEnemiesCombat();
        for (int i = 0; i < enemiesCombat.Count; i++)
        {
            enemiesCombat[i].SetProjectilesSpeed(slowMissilesSpeed);
        }
    }

    private void PowerupExpired()
    {
        activeProfile.SetUsingPowerup(false);
        RemovePowerupEffects();
    }

    IEnumerator PowerupDurationTick()
    {
        while (activeProfile.GetPowerupRemainingSeconds() > 0)
        {
            int remainingTime = activeProfile.GetPowerupRemainingSeconds() - 1;
            activeProfile.SetPowerupRemainingSeconds(remainingTime);
            yield return new WaitForSeconds(1f);
        }
        PowerupExpired();
        yield return null;
    }

    public void PowerupPickedUp()
    {
        if (activeProfile.GetUsingPowerup()) return;
        switch (powerup.tag)
        {
            case "RapidfirePowerup":
                UseRapidfirePowerup();
                break;
            case "ClonePowerup":
                UseClonePowerup();
                break;
            case "SlowMissilesPowerup":
                UseSlowMissilesPowerup();
                break;
            default:
                return;
        }
        activeProfile.SetActivePowerup(powerup.tag);
        activeProfile.SetUsingPowerup(true);
        activeProfile.SetPowerupRemainingSeconds(powerupDuration);
        StartCoroutine(PowerupDurationTick());
    }
}
