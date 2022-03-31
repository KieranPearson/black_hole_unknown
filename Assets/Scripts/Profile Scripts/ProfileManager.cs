using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager instance { get; private set; }

    private static List<Profile> profiles;

    [SerializeField] private string profilesFileName;

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
        profiles = new List<Profile>();

    }

    private void Start()
    {
        LoadProfiles();
    }
}
