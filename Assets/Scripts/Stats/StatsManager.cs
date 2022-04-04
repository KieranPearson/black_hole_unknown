using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DisplayStats))]
public class StatsManager : MonoBehaviour
{
    [SerializeField] private int scoreForEnemy;

    public static StatsManager instance { get; private set; }

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
    }

    private void ChangeLives(int amount)
    {
        int lives = activeProfile.GetLives();
        activeProfile.SetLives(lives + amount);
        displayStats.UpdateLives();
    }

    private void LevelManager_OnLevelLoaded()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();
        displayStats.UpdateAll();
    }

    private void EnemyCollisionHandler_OnEnemyDestroyed(GameObject enemy)
    {
        ChangeScore(scoreForEnemy);
    }

    void OnEnable()
    {
        LevelManager.OnLevelLoaded += LevelManager_OnLevelLoaded;
        EnemyCollisionHandler.OnEnemyDestroyed += EnemyCollisionHandler_OnEnemyDestroyed;
    }

    void OnDisable()
    {
        LevelManager.OnLevelLoaded -= LevelManager_OnLevelLoaded;
        EnemyCollisionHandler.OnEnemyDestroyed -= EnemyCollisionHandler_OnEnemyDestroyed;
    }
}
