using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private Queue<Achievement> achievementsQueue = new Queue<Achievement>();
    private bool displayingAchievement = false;
    private Profile activeProfile;
    private Achievement[] achievements;

    private void Start()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();
        if (Achievements.instance == null)
        {
            GameObject achievementsObject = new GameObject();
            achievementsObject.name = "Achievements";
            Achievements newAchievements = achievementsObject.AddComponent<Achievements>();
            achievements = newAchievements.GetAchievements();
        }
        else
        {
            achievements = Achievements.instance.GetAchievements();
        }
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

    private void UnlockAchievement(string achievementName)
    {
        if (!AchievementExists(achievementName)) return;
        if (AchievementOwned(achievementName)) return;
        activeProfile.AddAchievement(achievementName);
        Achievement unlockedAchievement = GetAchievementByName(achievementName);
        achievementsQueue.Enqueue(unlockedAchievement);
    }

    void OnEnable()
    {
        //AsteroidCollisionHandler.OnAchievementUnlocked += UnlockAchievement;
    }

    void OnDisable()
    {
        //AsteroidCollisionHandler.OnAchievementUnlocked -= UnlockAchievement;
    }
}
