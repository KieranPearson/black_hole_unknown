using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; private set; }

    [SerializeField] private int defaultResolutionWidth;
    [SerializeField] private int defaultResolutionHeight;
    [SerializeField] private int defaultResolutionRefreshRate;
    [SerializeField] private bool defaultFullscreen;

    [SerializeField] private string resolutionWidthPrefKey;
    [SerializeField] private string resolutionHeightPrefKey;
    [SerializeField] private string isFullscreenPrefKey;

    private Resolution[] resolutions; // width, height, refresh_rate
    private Resolution currentResolution;
    private bool isFullscreen;

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

    private void SetDefaultResolution()
    {
        currentResolution = new Resolution();
        currentResolution.width = defaultResolutionWidth;
        currentResolution.height = defaultResolutionHeight;
        currentResolution.refreshRate = defaultResolutionRefreshRate;
    }

    private Resolution? GetResolutionByWidthHeight(int width, int height)
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == width && resolutions[i].height == height)
            {
                return resolutions[i];
            }
        }
        return null;
    }

    private void LoadResolution()
    {
        SetDefaultResolution();
        resolutions = Screen.resolutions;
        if (resolutions.Length > 0)
        {
            currentResolution = resolutions[resolutions.Length - 1];
        }
        if (PlayerPrefs.HasKey(resolutionWidthPrefKey) && PlayerPrefs.HasKey(resolutionHeightPrefKey))
        {
            int loadedWidth = PlayerPrefs.GetInt(resolutionWidthPrefKey);
            int loadedHeight = PlayerPrefs.GetInt(resolutionHeightPrefKey);
            Resolution? loadedResolution = GetResolutionByWidthHeight(loadedWidth, loadedHeight);
            if (loadedResolution == null) return;
            currentResolution = (Resolution)loadedResolution;
        }
    }

    public Resolution[] GetResolutions()
    {
        return resolutions;
    }

    public void RequestResolutionChange(int width, int height)
    {
        Resolution? requestedResolution = GetResolutionByWidthHeight(width, height);
        if (requestedResolution == null) return;
        currentResolution = (Resolution)requestedResolution;
        UpdateDisplay();
    }

    private void LoadFullscreen()
    {
        isFullscreen = defaultFullscreen;
        if (PlayerPrefs.HasKey(isFullscreenPrefKey))
        {
            int loadedFullscreen = PlayerPrefs.GetInt(isFullscreenPrefKey);
            if (loadedFullscreen == 1)
            {
                isFullscreen = true;
                return;
            }
            isFullscreen = false;
        }
    }

    private void UpdateDisplay()
    {
        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullscreen);
    }

    private void Start()
    {
        LoadResolution();
        LoadFullscreen();
        UpdateDisplay();
    }
}
