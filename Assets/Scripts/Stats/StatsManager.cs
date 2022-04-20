using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DisplayStats))]
public class StatsManager : MonoBehaviour
{
    [SerializeField] private int scoreForEnemy;

    public static StatsManager instance { get; private set; }

    public static event System.Action OnAllLivesLost;
    public static event System.Action<string> OnAchievementUnlocked;

    private Profile activeProfile;
    private DisplayStats displayStats;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            displayStats = GetComponent<DisplayStats>();
        }
    }

    private void UpdateHighscore()
    {
        int score = activeProfile.GetCurrentScore();
        int highscore = activeProfile.GetHighscore();
        if (score <= highscore) return;
        activeProfile.SetHighscore(score);
    }

    private void ChangeScore(int amount)
    {
        int score = activeProfile.GetCurrentScore();
        activeProfile.SetCurrentScore(score + amount);
        displayStats.UpdateScore();
        UpdateHighscore();
    }

    private void ChangeLevel(int amount)
    {
        int level = activeProfile.GetLevel();
        activeProfile.SetLevel(level + amount);
        displayStats.UpdateLevel();
        switch (activeProfile.GetLevel())
        {
            case 2:
                OnAchievementUnlocked?.Invoke("Level 2");
                break;
            case 4:
                OnAchievementUnlocked?.Invoke("Level 4");
                break;
            case 8:
                OnAchievementUnlocked?.Invoke("Level 8");
                break;
            default:
                break;
        }
    }

    private void ChangeLives(int amount)
    {
        int lives = activeProfile.GetLives();
        activeProfile.SetLives(lives + amount);
        displayStats.UpdateLives();
        if (activeProfile.GetLives() <= 0)
        {
            OnAllLivesLost?.Invoke();
        }
    }

    private void ResetGameStats()
    {
        activeProfile.SetLevel(1);
        if (MultiplayerManager.instance.isMultiplayerModeEnabled())
        {
            activeProfile.SetLives(6);
        }
        else
        {
            activeProfile.SetLives(3);
        }
        activeProfile.SetCurrentScore(0);
        displayStats.UpdateAll();
    }

    private void LevelManager_OnLevelLoaded()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();
        displayStats.UpdateAll();
    }

    private void LevelManager_OnNewLevelStarted()
    {
        ChangeLevel(1);
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        ChangeScore(scoreForEnemy);
    }

    private void LevelManager_OnGameReset()
    {
        ResetGameStats();
    }

    private void PlayerCollisionHandler_OnPlayerHit()
    {
        ChangeLives(-1);
    }

    void OnEnable()
    {
        LevelManager.OnLevelLoaded += LevelManager_OnLevelLoaded;
        LevelManager.OnNewLevelStarted += LevelManager_OnNewLevelStarted;
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
        LevelManager.OnGameReset += LevelManager_OnGameReset;
        PlayerCollisionHandler.OnPlayerHit += PlayerCollisionHandler_OnPlayerHit;
    }

    void OnDisable()
    {
        LevelManager.OnLevelLoaded -= LevelManager_OnLevelLoaded;
        LevelManager.OnNewLevelStarted -= LevelManager_OnNewLevelStarted;
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
        LevelManager.OnGameReset -= LevelManager_OnGameReset;
        PlayerCollisionHandler.OnPlayerHit -= PlayerCollisionHandler_OnPlayerHit;
    }
}
