using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ProfileRemover
{
    public static void DeleteProfile(Profile profile)
    {
        if (File.Exists(GetPath(profile.GetName())))
        {
            File.Delete(GetPath(profile.GetName()));
        }
    }

    private static string GetPath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".save");
    }
}
