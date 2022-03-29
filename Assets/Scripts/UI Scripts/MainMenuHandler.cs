using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    private UIController uiController;

    private void Awake()
    {
        uiController = transform.parent.gameObject.GetComponent<UIController>();
    }

    public void PlayButtonClicked()
    {
        uiController.DisplayProfileSelector();
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
