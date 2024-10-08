using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance { get; private set; }

    [SerializeField] private int defaultResolutionWidth;
    [SerializeField] private int defaultResolutionHeight;
    [SerializeField] private int defaultRefreshRate;
    [SerializeField] private bool defaultFullscreen;
    [SerializeField] private float defaultMusicVolume;
    [SerializeField] private float defaultSoundEffectsVolume;
    [SerializeField] private bool defaultBonusLevels;

    [SerializeField] private string resolutionWidthPrefKey;
    [SerializeField] private string resolutionHeightPrefKey;
    [SerializeField] private string isFullscreenPrefKey;
    [SerializeField] private string musicVolumePrefKey;
    [SerializeField] private string soundEffectsVolumePrefKey;
    [SerializeField] private string refreshRatePrefKey;
    [SerializeField] private string bonusLevelsPrefKey;

    [SerializeField] private bool forceVSync;

    public static event System.Action OnSettingsLoaded;

    private Resolution[] resolutions; // width, height, refresh_rate
    private Resolution currentResolution;
    private int currentRefreshRate;
    private bool isFullscreen;
    private float musicVolume;
    private float soundEffectsVolume;
    private bool changesMade;
    private bool bonusLevels;

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
        musicVolume = defaultMusicVolume;
        soundEffectsVolume = defaultSoundEffectsVolume;
    }

    private void SetDefaultResolution()
    {
        currentResolution = new Resolution();
        currentResolution.width = defaultResolutionWidth;
        currentResolution.height = defaultResolutionHeight;
        currentResolution.refreshRate = defaultRefreshRate;
    }

    private Resolution? GetResolutionByWidthHeight(int width, int height, int refreshRate)
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width == width && resolutions[i].height == height && 
                resolutions[i].refreshRate == refreshRate)
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
            Resolution? loadedResolution = GetResolutionByWidthHeight(loadedWidth, loadedHeight, currentRefreshRate);
            if (loadedResolution == null) return;
            currentResolution = (Resolution)loadedResolution;
        }
    }

    public Resolution[] GetResolutions()
    {
        return resolutions;
    }

    public void RequestResolutionChange(int width, int height, int refreshRate)
    {
        Resolution? requestedResolution = GetResolutionByWidthHeight(width, height, refreshRate);
        if (requestedResolution == null) return;
        currentResolution = (Resolution)requestedResolution;
        currentRefreshRate = refreshRate;
        UpdateDisplay();
        changesMade = true;
    }

    public void RequestMusicVolumeChange(float musicVolume)
    {
        this.musicVolume = musicVolume;
        UpdateMusicVolume();
        changesMade = true;
    }

    public void RequestSoundVolumeChange(float soundVolume)
    {
        soundEffectsVolume = soundVolume;
        UpdateSoundEffectsVolume();
        changesMade = true;
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

    private void LoadBonusLevels()
    {
        bonusLevels = defaultBonusLevels;
        if (PlayerPrefs.HasKey(bonusLevelsPrefKey))
        {
            int loadedBonusLevels = PlayerPrefs.GetInt(bonusLevelsPrefKey);
            if (loadedBonusLevels == 1)
            {
                bonusLevels = true;
                return;
            }
            bonusLevels = false;
        }
    }

    public bool IsFullscreen()
    {
        return isFullscreen;
    }

    public bool BonusLevelsEnabled()
    {
        return bonusLevels;
    }

    public void RequestFullscreenChange(bool isFullscreen)
    {
        if (this.isFullscreen == isFullscreen) return;
        this.isFullscreen = isFullscreen;
        UpdateDisplay();
        changesMade = true;
    }

    public void RequestBonusLevelsChange(bool bonusLevels)
    {
        if (this.bonusLevels == bonusLevels) return;
        this.bonusLevels = bonusLevels;
        changesMade = true;
    }

    private void UpdateDisplay()
    {
        Screen.SetResolution(currentResolution.width, currentResolution.height, isFullscreen, currentRefreshRate);
        if (forceVSync)
        {
            QualitySettings.vSyncCount = 1;
        }
    }

    private void LoadMusicVolume()
    {
        musicVolume = defaultMusicVolume;
        if (PlayerPrefs.HasKey(musicVolumePrefKey))
        {
            float loadedMusicVolume = PlayerPrefs.GetFloat(musicVolumePrefKey);
            if (loadedMusicVolume > 1f || loadedMusicVolume < 0f) loadedMusicVolume = 0f;
            musicVolume = loadedMusicVolume;
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    private void LoadSoundEffectsVolume()
    {
        soundEffectsVolume = defaultSoundEffectsVolume;
        if (PlayerPrefs.HasKey(soundEffectsVolumePrefKey))
        {
            float loadedSoundEffectsVolume = PlayerPrefs.GetFloat(soundEffectsVolumePrefKey);
            if (loadedSoundEffectsVolume > 1 || loadedSoundEffectsVolume < 0) return;
            soundEffectsVolume = loadedSoundEffectsVolume;
        }
    }

    public float GetSoundEffectsVolume()
    {
        return soundEffectsVolume;
    }

    private void UpdateMusicVolume()
    {
        BackgroundMusic.instance.UpdateMusicVolume(musicVolume);
    }

    private void UpdateSoundEffectsVolume()
    {

    }

    public Resolution GetCurrentResolution()
    {
        return currentResolution;
    }

    private void LoadRefreshRate()
    {
        currentRefreshRate = defaultRefreshRate;
        if (PlayerPrefs.HasKey(refreshRatePrefKey))
        {
            currentRefreshRate = PlayerPrefs.GetInt(refreshRatePrefKey);
        }
    }

    private void Start()
    {
        LoadRefreshRate();
        LoadResolution();
        LoadFullscreen();
        UpdateDisplay();
        LoadMusicVolume();
        UpdateMusicVolume();
        LoadSoundEffectsVolume();
        LoadBonusLevels();
        OnSettingsLoaded?.Invoke();
    }

    private void SaveSettings()
    {
        if (!changesMade) return;
        PlayerPrefs.SetInt(resolutionWidthPrefKey, currentResolution.width);
        PlayerPrefs.SetInt(resolutionHeightPrefKey, currentResolution.height);
        PlayerPrefs.SetInt(refreshRatePrefKey, currentRefreshRate);
        PlayerPrefs.SetInt(isFullscreenPrefKey, isFullscreen ? 1 : 0);
        PlayerPrefs.SetFloat(musicVolumePrefKey, musicVolume);
        PlayerPrefs.SetFloat(soundEffectsVolumePrefKey, soundEffectsVolume);
        PlayerPrefs.SetInt(bonusLevelsPrefKey, bonusLevels ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }
}
