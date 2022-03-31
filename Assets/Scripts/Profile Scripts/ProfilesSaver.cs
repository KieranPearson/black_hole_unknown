using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class ProfilesSaver
{
    public static void SaveProfile()
    {
        FileStream stream = new FileStream(GetSavePath(), FileMode.Create);
    }

    private static string GetSavePath()
    {
        return Path.Combine(Application.persistentDataPath, "test.save");
    }
}
