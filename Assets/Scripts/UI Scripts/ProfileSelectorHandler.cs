using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSelectorHandler : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject createSlotPrefab;
    [SerializeField] private Transform content;

    private void PopulateProfilesList()
    {
        List<Profile> profiles = ProfileManager.instance.GetProfiles();
        for (int i = 0; i < profiles.Count; i++)
        {
            GameObject slot = Instantiate(slotPrefab);
            slot.name = profiles[i].GetName() + "Slot";
            ProfileSlot profileSlot = slot.GetComponent<ProfileSlot>();
            profileSlot.SetName(profiles[i].GetName());
            profileSlot.SetHighscore(profiles[i].GetHighscore());
            slot.transform.SetParent(content, false);
        }
        GameObject newProfileSlot = Instantiate(createSlotPrefab);
        newProfileSlot.name = "NewProfileSlot";
        newProfileSlot.transform.SetParent(content, false);
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
}
