using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private Queue<string> achievementsQueue = new Queue<string>();
    private bool displayingAchievement = false;
    private Profile activeProfile;

    private void Start()
    {
        activeProfile = ProfileManager.instance.GetActiveProfile();
    }

    private void UnlockAchievement(string achievementName)
    {

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
