using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ProfileSaver
{
    public static void SaveProfile(Profile profile)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        using (FileStream stream = new FileStream(GetSavePath(profile.GetName()), FileMode.OpenOrCreate))
        {
            try
            {
                formatter.Serialize(stream, profile);
            }
            catch (Exception)
            {
                return;
            }
        }
    }

    private static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".save");
    }
}
