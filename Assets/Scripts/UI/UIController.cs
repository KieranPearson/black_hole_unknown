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
    [SerializeField] private float transitionSpeed;

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
    private CanvasGroup transitionTo;

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
        mainMenuGroup.alpha = 0;
        mainMenu.SetActive(true);
        transitionTo = mainMenuGroup;
        activeTransition = Transition.In;
    }

    private void DisplayMenu(CanvasGroup canvasGroup)
    {
        if (activeMenuGroup == canvasGroup) return;
        if (activeTransition == Transition.None)
        {
            transitionTo = canvasGroup;
            activeTransition = Transition.Out;
        }
    }

    public void DisplayMainMenu()
    {
        DisplayMenu(mainMenuGroup);
    }

    public void DisplayProfileSelector()
    {
        DisplayMenu(profileSelectorGroup);
    }

    public void DisplayAchievements()
    {
        DisplayMenu(achievementsGroup);
    }

    public void DispalySettings()
    {
        DisplayMenu(settingsGroup);
    }

    public void DisplayCredits()
    {
        DisplayMenu(creditsGroup);
    }

    private void Update()
    {
        TransitionMenu();
    }

    private void TransitionMenu()
    {
        if (activeTransition == Transition.Out)
        {
            TransitionOutMenu();
        } 
        else if (activeTransition == Transition.In)
        {
            TransitionInMenu();
        }
    }

    private void TransitionOutMenu()
    {
        if (activeMenuGroup.alpha > 0)
        {
            activeMenuGroup.alpha -= (Time.deltaTime * transitionSpeed);
        } else
        {
            GetMenuGroupGameObject(activeMenuGroup).SetActive(false);
            transitionTo.alpha = 0;
            GetMenuGroupGameObject(transitionTo).SetActive(true);
            activeTransition = Transition.In;
        }
    }

    private void TransitionInMenu()
    {
        if (transitionTo.alpha < 1)
        {
            transitionTo.alpha += (Time.deltaTime * transitionSpeed);
        } else
        {
            activeMenuGroup = transitionTo;
            transitionTo = null;
            activeTransition = Transition.None;
        }
    }

    private GameObject GetMenuGroupGameObject(CanvasGroup canvasGroup)
    {
        if (canvasGroup == mainMenuGroup)
        {
            return mainMenu;
        }
        else if (canvasGroup == profileSelectorGroup)
        {
            return profileSelector;
        }
        else if (canvasGroup == achievementsGroup)
        {
            return achievements;
        }
        else if (canvasGroup == settingsGroup)
        {
            return settings;
        }
        else if (canvasGroup == creditsGroup)
        {
            return credits;
        }
        return null;
    }
}
