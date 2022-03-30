using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingsHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private int resolutionWidth;
    private int resolutionHeight;
    private bool isFullscreen;
    private float musicVolume;
    private float soundEffectsVolume;

    public void ApplySettings()
    {

    }

    private void PopulateResolutionDropdown()
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

    private void SettingsManager_OnSettingsLoaded()
    {
        PopulateResolutionDropdown();
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
