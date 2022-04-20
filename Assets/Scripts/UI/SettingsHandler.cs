using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle isFullscreenToggle;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundEffectsVolumeSlider;
    [SerializeField] private Toggle bonusLevelsToggle;

    private SettingsManager settingsManager;
    private bool settingsLoaded;

    private void Awake()
    {
        settingsManager = SettingsManager.instance;
    }

    void Start()
    {
        if (!settingsLoaded)
        {
            SettingsManager_OnSettingsLoaded();
        }
    }

    public void ApplyButtonClicked()
    {
        string selected = resolutionDropdown.options[resolutionDropdown.value].text;
        string[] resolutionValues = selected.Split('x');
        if (resolutionValues.Length == 2)
        {
            int resWidth;
            int resHeight;
            int refreshRate;
            int.TryParse(resolutionValues[0], out resWidth);
            string[] heightAndRefreshRate = resolutionValues[1].Split('@');
            int.TryParse(heightAndRefreshRate[0], out resHeight);
            int.TryParse(heightAndRefreshRate[1], out refreshRate);
            if (resWidth != 0 && resHeight != 0)
            {
                settingsManager.RequestResolutionChange(resWidth, resHeight, refreshRate);
            }
        }
        settingsManager.RequestFullscreenChange(isFullscreenToggle.isOn);
        settingsManager.RequestBonusLevelsChange(bonusLevelsToggle.isOn);
    }

    private void UpdateResolutionDropdown()
    {
        Resolution[] resolutions = settingsManager.GetResolutions();
        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            string resolutionText = resolutions[i].width + "x" + resolutions[i].height + "@" + resolutions[i].refreshRate;
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = resolutionText });
        }
        Resolution currentResolution = settingsManager.GetCurrentResolution();
        string currentResolutionText = currentResolution.width + "x" + currentResolution.height + "@" + 
            currentResolution.refreshRate;
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == currentResolutionText);
    }

    private void UpdateIsFullscreen()
    {
        isFullscreenToggle.isOn = settingsManager.IsFullscreen();
    }

    private void UpdateBonusLevels()
    {
        bonusLevelsToggle.isOn = settingsManager.BonusLevelsEnabled();
    }

    private void UpdateMusicVolume()
    {
        musicVolumeSlider.value = settingsManager.GetMusicVolume();
    }

    private void UpdateSoundEffectsVolume()
    {
        soundEffectsVolumeSlider.value = settingsManager.GetSoundEffectsVolume();
    }

    private void SettingsManager_OnSettingsLoaded()
    {
        UpdateResolutionDropdown();
        UpdateIsFullscreen();
        UpdateMusicVolume();
        UpdateSoundEffectsVolume();
        UpdateBonusLevels();
        settingsLoaded = true;
    }

    void OnEnable()
    {
        SettingsManager.OnSettingsLoaded += SettingsManager_OnSettingsLoaded;
    }

    void OnDisable()
    {
        SettingsManager.OnSettingsLoaded -= SettingsManager_OnSettingsLoaded;
    }
}
