using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance { get; private set; }

    public static event System.Action OnProfilesLoaded;

    private List<Profile> profiles;
    private List<Profile> updatedProfiles;
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
        updatedProfiles = new List<Profile>();
        OnProfilesLoaded?.Invoke();
    }

    private void SaveProfiles()
    {
        for (int i = 0; i < updatedProfiles.Count; i++)
        {
            ProfileSaver.SaveProfile(updatedProfiles[i]);
        }
    }

    public List<Profile> GetProfiles()
    {
        return profiles;
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
