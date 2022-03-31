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
    }

    private void SaveProfiles()
    {
        for (int i = 0; i < updatedProfiles.Count; i++)
        {

        }
    }

    private void Start()
    {
        LoadProfiles();
    }
}
