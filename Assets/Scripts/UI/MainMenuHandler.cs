using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    public static event System.Action OnMultiplayerModeClicked;

    private UIController uiController;

    private void Awake()
    {
        uiController = GetComponent<UIController>();
    }

    public void PlayButtonClicked()
    {
        uiController.DisplayProfileSelector();
    }

    public void MultiplayerButtonClicked()
    {
        OnMultiplayerModeClicked?.Invoke();
    }

    public void AchievementsButtonClicked()
    {
        uiController.DisplayAchievements();
    }

    public void SettingsButtonClicked()
    {
        uiController.DispalySettings();
    }

    public void CreditsButtonClicked()
    {
        uiController?.DisplayCredits();
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }
}
