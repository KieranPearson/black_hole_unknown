using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayStats : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text livesText;

    public void UpdateScore()
    {
        int score = ProfileManager.instance.GetActiveProfile().GetCurrentScore();
        scoreText.text = "SCORE\n" + score.ToString("N0");
    }

    public void UpdateLevel()
    {
        int level = ProfileManager.instance.GetActiveProfile().GetLevel();
        levelText.text = "LEVEL " + level.ToString("N0");
    }

    public void UpdateLives()
    {
        int lives = ProfileManager.instance.GetActiveProfile().GetLives();
        livesText.text = "LIVES\n" + lives.ToString();
    }

    public void UpdateAll()
    {
        UpdateScore();
        UpdateLevel();
        UpdateLives();
    }
}
