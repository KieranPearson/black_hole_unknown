using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance { get; private set; }

    public static event System.Action OnProfilesLoaded;

    private List<Profile> profiles;
    private List<Profile> profilesToSave;
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
            DontDestroyOnLoad(gameObject);
        }
    }

    private void LoadProfiles()
    {
        profiles = ProfilesLoader.LoadProfiles();
        profilesToSave = new List<Profile>();
        OnProfilesLoaded?.Invoke();
    }

    private void SaveProfiles()
    {
        for (int i = 0; i < profilesToSave.Count; i++)
        {
            ProfileSaver.SaveProfile(profilesToSave[i]);
        }
    }

    public List<Profile> GetProfiles()
    {
        return profiles;
    }

    private Profile GetProfileByName(string profileName)
    {
        for (int i = 0; i < profiles.Count; i++)
        {
            if (profiles[i].GetName() == profileName)
            {
                return profiles[i];
            }
        }
        return null;
    }

    private bool IsAProfileToSave(Profile profile)
    {
        return profilesToSave.Contains(profile);
    }

    private void MarkProfileToSave(Profile profile)
    {
        if (IsAProfileToSave(profile)) return;
        profilesToSave.Add(profile);
    }

    private void ProfileSlotNew_OnNewProfile(string profileName)
    {
        if (GetProfileByName(profileName) != null) return;
        Profile newProfile = new Profile(profileName);
        profiles.Add(newProfile);
        MarkProfileToSave(newProfile);
        activeProfile = newProfile;
        // start game
    }

    private void ProfileSlot_OnProfileSelected(string profileName)
    {
        Profile selectedProfile = GetProfileByName(profileName);
        if (selectedProfile == null) return;
        MarkProfileToSave(selectedProfile);
        activeProfile = selectedProfile;
        // start game
    }

    void OnEnable()
    {
        ProfileSlotNew.OnNewProfile += ProfileSlotNew_OnNewProfile;
        ProfileSlot.OnProfileSelected += ProfileSlot_OnProfileSelected;
    }

    void OnDisable()
    {
        ProfileSlotNew.OnNewProfile -= ProfileSlotNew_OnNewProfile;
        ProfileSlot.OnProfileSelected -= ProfileSlot_OnProfileSelected;
    }

    private void Start()
    {
        LoadProfiles();
    }

    private void OnApplicationQuit()
    {
        SaveProfiles();
    }
}
