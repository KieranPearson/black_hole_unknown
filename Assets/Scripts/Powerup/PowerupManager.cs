using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PowerupManager : MonoBehaviour
{
    [SerializeField] private GameObject powerup;
    [SerializeField] private int percentChanceOfPowerup;
    [SerializeField] private GameObject playerClone;
    [SerializeField] private GameObject player2Clone;
    [SerializeField] private Combat playerCombat;
    [SerializeField] private Combat player2Combat;
    [SerializeField] private Combat enemyCombat;
    [SerializeField] private int powerupDuration;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform player2Transform;
    [SerializeField] private float rapidfireSpeed;
    [SerializeField] private float slowMissilesSpeed;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private BoxCollider2D player2Collider;
    [SerializeField] private SpriteFader playerCloneSpriteFader;
    [SerializeField] private SpriteFader player2CloneSpriteFader;
    [SerializeField] private AudioClip powerupCollectedSound;

    public static PowerupManager instance { get; private set; }

    public static event System.Action<AudioSource> OnPlayCollectedSound;

    private Transform powerupTransform;
    private PowerupMovement powerupMovement;
    private PowerupTypeChanger powerupTypeChanger;
    private enum PowerupType {
        RapidfirePowerup,
        ClonePowerup,
        SlowMissilesPowerup
    };
    private Profile activeProfile;
    private Combat playerCloneCombat;
    private Movement playerCloneMovement;
    private PlayerController playerCloneController;
    private Combat player2CloneCombat;
    private Movement player2CloneMovement;
    private PlayerController player2CloneController;
    private MultiplayerManager multiplayerManager;
    private AudioSource audioSource;

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
        playerCloneCombat = playerClone.GetComponent<Combat>();
        playerCloneMovement = playerClone.GetComponent<Movement>();
        playerCloneController = playerClone.GetComponent<PlayerController>();
        player2CloneCombat = player2Clone.GetComponent<Combat>();
        player2CloneMovement = player2Clone.GetComponent<Movement>();
        player2CloneController = player2Clone.GetComponent<PlayerController>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Start()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();
        multiplayerManager = MultiplayerManager.instance;
        audioSource.clip = powerupCollectedSound;
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
        if (multiplayerManager.isMultiplayerModeEnabled() &&
            SettingsManager.instance.BonusLevelsEnabled()) return;
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
        powerup.tag = powerupName;
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

    private void LoadPowerupInLevel()
    {
        string activePowerup = activeProfile.GetActivePowerup();
        if (activePowerup == "None") return;
        float powerupXPosition = activeProfile.GetPowerupXPosition();
        float powerupYPosition = activeProfile.GetPowerupYPosition();
        DisplayPowerup(new Vector2(powerupXPosition, powerupYPosition), activePowerup);
    }

    private void LoadPowerupInUse()
    {
        if (!activeProfile.GetUsingPowerup()) return;
        EnablePowerupEffect();
    }

    public void LoadPowerup()
    {
        LoadPowerupInLevel();
        LoadPowerupInUse();
    }

    private void DisablePlayer2Clone()
    {
        if (!player2Clone.activeSelf) return;
        player2CloneMovement.Stop();
        Command stopMovingLeft = new StopMovingLeftCommand(player2CloneController);
        Command stopMovingRight = new StopMovingRightCommand(player2CloneController);
        player2CloneController.UpdateState(stopMovingLeft);
        player2CloneController.UpdateState(stopMovingRight);
        player2CloneCombat.ToggleFire(false);
        player2Clone.SetActive(false);
    }

    private void DisablePlayerClone()
    {
        if (!playerClone.activeSelf) return;
        playerCloneMovement.Stop();
        Command stopMovingLeft = new StopMovingLeftCommand(playerCloneController);
        Command stopMovingRight = new StopMovingRightCommand(playerCloneController);
        playerCloneController.UpdateState(stopMovingLeft);
        playerCloneController.UpdateState(stopMovingRight);
        playerCloneCombat.ToggleFire(false);
        playerClone.SetActive(false);
    }

    private void RemovePowerupEffects()
    {
        playerCombat.SetDefaultFireRate();
        DisablePlayerClone();
        if (multiplayerManager.isMultiplayerModeEnabled())
        {
            player2Combat.SetDefaultFireRate();
            DisablePlayer2Clone();
        }
        List<Combat> enemiesCombat = LevelManager.instance.GetAllEnemiesCombat();
        for (int i = 0; i < enemiesCombat.Count; i++)
        {
            enemiesCombat[i].SetProjectilesDefaultSpeed();
        }
        playerCollider.enabled = true;
        player2Collider.enabled = true;

        List<ProjectileMovement> loadedEnemyProjectileMovements = ProjectileLoader.instance.GetEnemyProjectileMovements();
        if (loadedEnemyProjectileMovements.Count <= 0) return;
        float enemyProjectileSpeed = enemyCombat.GetProjectilesDefaultSpeed();
        if (enemyCombat.ProjectilesDefaultMoveDown())
        {
            enemyProjectileSpeed = -enemyProjectileSpeed;
        }
        for (int i = 0; i < loadedEnemyProjectileMovements.Count; i++)
        {
            loadedEnemyProjectileMovements[i].SetSpeed(enemyProjectileSpeed);
        }
    }

    private void UseRapidfirePowerup()
    {
        playerCombat.SetFireRate(rapidfireSpeed);
        if (multiplayerManager.isMultiplayerModeEnabled())
        {
            player2Combat.SetFireRate(rapidfireSpeed);
        }
    }

    private void UseClonePowerup()
    {
        Vector3 playerPosition = playerTransform.position;
        Vector3 playerClonePosition = playerClone.transform.position;
        playerClone.transform.position = new Vector3(playerPosition.x - 1.75f, playerClonePosition.y, playerClonePosition.z);
        playerClone.SetActive(true);
        playerCloneSpriteFader.enabled = true;
        if (multiplayerManager.isMultiplayerModeEnabled())
        {
            Vector3 player2Position = player2Transform.position;
            Vector3 player2ClonePosition = player2Clone.transform.position;
            player2Clone.transform.position = new Vector3(player2Position.x - 1.75f, player2ClonePosition.y, player2ClonePosition.z);
            player2Clone.SetActive(true);
            player2CloneSpriteFader.enabled = true;
        }
    }

    private void UseSlowMissilesPowerup()
    {
        List<Combat> enemiesCombat = LevelManager.instance.GetAllEnemiesCombat();
        for (int i = 0; i < enemiesCombat.Count; i++)
        {
            enemiesCombat[i].SetProjectilesSpeed(slowMissilesSpeed);
        }
        playerCollider.enabled = false;
        player2Collider.enabled = false;

        List<ProjectileMovement> loadedEnemyProjectileMovements = ProjectileLoader.instance.GetEnemyProjectileMovements();
        if (loadedEnemyProjectileMovements.Count <= 0) return;
        float enemySlowProjectileSpeed = slowMissilesSpeed;
        if (enemyCombat.ProjectilesDefaultMoveDown())
        {
            enemySlowProjectileSpeed = -enemySlowProjectileSpeed;
        }
        for (int i = 0; i < loadedEnemyProjectileMovements.Count; i++)
        {
            loadedEnemyProjectileMovements[i].SetSpeed(enemySlowProjectileSpeed);
        }
    }

    private void PowerupExpired()
    {
        activeProfile.SetUsingPowerup(false);
        activeProfile.SetPowerupUsed("None");
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

    private void EnablePowerupEffect()
    {
        switch (activeProfile.GetPowerupUsed())
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
        StartCoroutine(PowerupDurationTick());
    }

    public void PowerupPickedUp()
    {
        if (activeProfile.GetUsingPowerup()) return;
        activeProfile.SetActivePowerup("None");
        activeProfile.SetPowerupUsed(powerup.tag);
        activeProfile.SetUsingPowerup(true);
        activeProfile.SetPowerupRemainingSeconds(powerupDuration);
        EnablePowerupEffect();
        OnPlayCollectedSound?.Invoke(audioSource);
    }
}
