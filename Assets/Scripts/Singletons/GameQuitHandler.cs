using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitHandler : MonoBehaviour
{
    public static GameQuitHandler instance { get; private set; }

    private static bool isAllowedToQuit = false;
    
    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public static void AllowQuitting()
    {
        isAllowedToQuit = true;
    }

    static bool WantsToQuit()
    {
        return isAllowedToQuit;
    }

    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;
    }
}
