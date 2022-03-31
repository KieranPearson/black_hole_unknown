using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ProfilesLoader
{
    public static List<Profile> LoadProfiles()
    {
        List<Profile> loadedProfiles = new List<Profile>();
        string[] profilePaths = Directory.GetFiles(Application.persistentDataPath, "*.save");
        for (int i = 0; i < profilePaths.Length; i++)
        {
            Debug.Log(profilePaths[i]);
        }
        return loadedProfiles;
    }
}
