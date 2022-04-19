using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerManager : MonoBehaviour
{
    private bool multiplayerModeEnabled;

    public static MultiplayerManager instance { get; private set; }

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

    public void ToggleMultiplayerMode(bool enabled)
    {
        multiplayerModeEnabled = enabled;
    }

    private void StartMultiplayerMode()
    {
        multiplayerModeEnabled = true;
        ProfileManager.instance.StartUnsavedGame();
    }

    private void OnMultiplayerModeClicked()
    {
        StartMultiplayerMode();
    }

    private void OnEnable()
    {
        MainMenuHandler.OnMultiplayerModeClicked += OnMultiplayerModeClicked;
    }

    private void OnDisable()
    {
        MainMenuHandler.OnMultiplayerModeClicked -= OnMultiplayerModeClicked;
    }
}
