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

    public void ApplyButtonClicked()
    {
        string[] resolutionValues = resolutionDropdown.value.ToString().Split('x');
        if (resolutionValues.Length == 2)
        {
            int resWidth;
            int resHeight;
            int.TryParse(resolutionValues[0], out resWidth);
            int.TryParse(resolutionValues[1], out resHeight);
            if (resWidth != 0 && resHeight != 0)
            {
                SettingsManager.instance.RequestResolutionChange(resWidth, resHeight);
            }
        }
        SettingsManager.instance.RequestFullscreenChange(isFullscreenToggle.isOn);
    }

    private void UpdateResolutionDropdown()
    {
        Resolution[] resolutions = SettingsManager.instance.GetResolutions();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionText = resolutions[i].width + "x" + resolutions[i].height;
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = resolutionText });
        }
        Resolution currentResolution = SettingsManager.instance.GetCurrentResolution();
        string currentResolutionText = currentResolution.width + "x" + currentResolution.height;
        resolutionDropdown.value = resolutionDropdown.options.FindIndex(option => option.text == currentResolutionText);
    }

    private void UpdateIsFullscreen()
    {
        isFullscreenToggle.isOn = SettingsManager.instance.IsFullscreen();
    }

    private void UpdateMusicVolume()
    {
        musicVolumeSlider.value = SettingsManager.instance.GetMusicVolume();
    }

    private void UpdateSoundEffectsVolume()
    {
        soundEffectsVolumeSlider.value = SettingsManager.instance.GetSoundEffectsVolume();
    }

    private void SettingsManager_OnSettingsLoaded()
    {
        UpdateResolutionDropdown();
        UpdateIsFullscreen();
        UpdateMusicVolume();
        UpdateSoundEffectsVolume();
    }

    void OnEnable()
    {
        SettingsManager.OnSettingsLoaded += SettingsManager_OnSettingsLoaded;
    }

    void OnDisable()
    {
        SettingsManager.OnSettingsLoaded -= SettingsManager_OnSettingsLoaded;
    }

    void Start()
    {
        
    }
}
