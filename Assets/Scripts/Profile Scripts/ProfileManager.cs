using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance { get; private set; }

    private List<Profile> profiles;
    private List<Profile> updatedProfiles;

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
    }

    private void SaveProfiles()
    {
        for (int i = 0; i < updatedProfiles.Count; i++)
        {
            ProfileSaver.SaveProfile(updatedProfiles[i]);
        }
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
