using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerModeSetup : MonoBehaviour
{
    [SerializeField] private GameObject achievementsManager;
    [SerializeField] private GameObject player2;

    private MultiplayerManager multiplayerManager;

    private void Start()
    {
        multiplayerManager = MultiplayerManager.instance;
        if (!multiplayerManager.isMultiplayerModeEnabled())
        {
            Destroy(gameObject);
            return;
        }
        SetupMultiplayerMode();
        ProfileManager.instance.GetActiveProfile().SetLives(6);
    }

    private void SetupMultiplayerMode()
    {
        achievementsManager.SetActive(false);
        player2.SetActive(true);
    }
}
