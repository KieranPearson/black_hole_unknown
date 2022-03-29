using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject profileSelector;
    [SerializeField] private GameObject achievements;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject credits;

    private CanvasGroup mainMenuGroup;
    private CanvasGroup profileSelectorGroup;
    private CanvasGroup achievementsGroup;
    private CanvasGroup settingsGroup;
    private CanvasGroup creditsGroup;

    private enum Transition
    {
        None,
        Out,
        In
    }

    private CanvasGroup activeMenuGroup;
    private Transition activeTransition;

    void Awake()
    {
        mainMenuGroup = mainMenu.GetComponent<CanvasGroup>();
        profileSelectorGroup = profileSelector.GetComponent<CanvasGroup>();
        achievementsGroup = achievements.GetComponent<CanvasGroup>();
        settingsGroup = settings.GetComponent<CanvasGroup>();
        creditsGroup = credits.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        activeMenuGroup = mainMenuGroup;
        activeTransition = Transition.None;
    }

    private void TransitionOut(CanvasGroup canvasGroup)
    {
        //mainMenuCanvasGroup.alpha = 0;
    }

    public void DisplayProfileSelector()
    {
        //TransitionOutMainMenu();
    }

    public void DisplayAchievements()
    {

    }

    public void DispalySettings()
    {

    }

    public void DisplayCredits()
    {

    }
}
