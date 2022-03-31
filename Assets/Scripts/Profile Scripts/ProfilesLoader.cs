using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ProfilesLoader
{
    public static List<Profile> LoadProfiles()
    {
        List<Profile> loadedProfiles = new List<Profile>();
        string[] profilePaths = Directory.GetFiles(Application.persistentDataPath, "*.save");
        BinaryFormatter formatter = new BinaryFormatter();
        for (int i = 0; i < profilePaths.Length; i++)
        {
            using (FileStream stream = new FileStream(profilePaths[i], FileMode.Open))
            {
                try
                {
                    Profile loadedProfile = formatter.Deserialize(stream) as Profile;
                    loadedProfiles.Add(loadedProfile);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        return loadedProfiles;
    }
}
