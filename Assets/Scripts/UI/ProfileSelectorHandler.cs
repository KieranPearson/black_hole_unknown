using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSelectorHandler : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject createSlotPrefab;
    [SerializeField] private Transform content;
    [SerializeField] private GameObject achievementIconPrefab;

    private bool hasPopulatedProfilesList;

    private void PopulateAchievementIcons(Transform slotTransform, Profile profile)
    {
        Transform achievementsTransform = slotTransform.Find("Achievements");
        List<string> unlockedAchievements = profile.GetAchievements();
        for (int i = 0; i < unlockedAchievements.Count; i++)
        {
            Achievement achievement = Achievements.instance.GetAchievementByName(unlockedAchievements[i]);
            GameObject achievementIcon = Instantiate(achievementIconPrefab);
            achievementIcon.name = "AchievementIcon";
            Image achievementIconImage = achievementIcon.GetComponent<Image>();
            achievementIconImage.sprite = achievement.GetIcon();
            Transform achievementIconTransform = achievementIcon.transform;
            achievementIconTransform.SetParent(achievementsTransform, false);
        }
    }

    private void PopulateProfilesList()
    {
        List<Profile> profiles = ProfileManager.instance.GetProfiles();
        for (int i = 0; i < profiles.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab);
            Transform slotTransform = slot.transform;
            slot.name = profiles[i].GetName() + "Slot";
            ProfileSlot profileSlot = slot.GetComponent<ProfileSlot>();
            profileSlot.SetName(profiles[i].GetName());
            profileSlot.SetHighscore(profiles[i].GetHighscore());
            slotTransform.SetParent(content, false);
            PopulateAchievementIcons(slotTransform, profiles[i]);
        }
        GameObject newProfileSlot = Instantiate(createSlotPrefab);
        newProfileSlot.name = "NewProfileSlot";
        newProfileSlot.transform.SetParent(content, false);
        hasPopulatedProfilesList = true;
    }

    private void ProfileManager_OnProfilesLoaded()
    {
        PopulateProfilesList();
    }

    void OnEnable()
    {
        ProfileManager.OnProfilesLoaded += ProfileManager_OnProfilesLoaded;
    }

    void OnDisable()
    {
        ProfileManager.OnProfilesLoaded -= ProfileManager_OnProfilesLoaded;
    }

    private void CheckProfilesPopulated()
    {
        if (hasPopulatedProfilesList) return;
        PopulateProfilesList();
    }

    private void Start()
    {
        CheckProfilesPopulated();
    }
}
