using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private GameObject achievementUI;
    [SerializeField] private TMP_Text achievementName;
    [SerializeField] private TMP_Text achievementDescription;
    [SerializeField] private Image achievementIcon;

    private Queue<Achievement> achievementsQueue = new Queue<Achievement>();
    private bool displayingAchievement = false;
    private Profile activeProfile;
    private Achievement[] achievements;

    private void Start()
    {
        achievements = Achievements.instance.GetAchievements();
        activeProfile = ProfileManager.instance.GetActiveProfile();
    }

    private bool AchievementExists(string achievementName)
    {
        for (int i = 0; i < achievements.Length; i++)
        {
            if (achievements[i].GetName() == achievementName) return true;
        }
        return false;
    }

    private bool AchievementOwned(string achievementName)
    {
        List<string> ownedAchievements = activeProfile.GetAchievements();
        for (int i = 0; i < ownedAchievements.Count; i++)
        {
            if (ownedAchievements[i] == achievementName) return true;
        }
        return false;
    }

    private Achievement GetAchievementByName(string achievementName)
    {
        for (int i = 0; i < achievements.Length; i++)
        {
            if (achievements[i].GetName() == achievementName) return achievements[i];
        }
        return null;
    }

    private void UpdateAchivementDisplay(Achievement achievement)
    {
        achievementName.text = achievement.GetName();
        achievementDescription.text = achievement.GetDescription();
        achievementIcon.sprite = achievement.GetIcon();
    }

    IEnumerator DisplayAchievements()
    {
        displayingAchievement = true;
        while (achievementsQueue.Count > 0)
        {
            Achievement achievementToDisplay = achievementsQueue.Dequeue();
            UpdateAchivementDisplay(achievementToDisplay);
            achievementUI.SetActive(true);
            while (achievementUI.activeSelf == true)
            {
                yield return new WaitForSeconds(1f);
            }
        }
        displayingAchievement = false;
        yield return null;
    }

    private void UnlockAchievement(string achievementName)
    {
        if (!AchievementExists(achievementName)) return;
        if (AchievementOwned(achievementName)) return;
        activeProfile.AddAchievement(achievementName);
        Achievement unlockedAchievement = GetAchievementByName(achievementName);
        achievementsQueue.Enqueue(unlockedAchievement);
        if (!displayingAchievement) StartCoroutine(DisplayAchievements());
    }

    void OnEnable()
    {
        PowerupCollisionHandler.OnAchievementUnlocked += UnlockAchievement;
        StatsManager.OnAchievementUnlocked += UnlockAchievement;
        LevelManager.OnAchievementUnlocked += UnlockAchievement;
    }

    void OnDisable()
    {
        PowerupCollisionHandler.OnAchievementUnlocked -= UnlockAchievement;
        StatsManager.OnAchievementUnlocked -= UnlockAchievement;
        LevelManager.OnAchievementUnlocked -= UnlockAchievement;
    }
}
