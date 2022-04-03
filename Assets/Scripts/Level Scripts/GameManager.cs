using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject screenFader;

    private Fader fader;

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        fader = screenFader.GetComponent<Fader>();
    }

    public void StartGame()
    {
        fader.ToggleFadeIn();
        screenFader.SetActive(true);
    }

    private void Fader_OnFadedOut()
    {
        SceneManager.LoadScene(sceneBuildIndex:1); // load game scene
    }

    private void GameQuitHandler_OnRequestDataSync()
    {
        GameQuitHandler.DataSynced();
    }

    void OnEnable()
    {
        Fader.OnScreenFadedOut += Fader_OnFadedOut;
        GameQuitHandler.OnRequestDataSync += GameQuitHandler_OnRequestDataSync;
    }

    void OnDisable()
    {
        Fader.OnScreenFadedOut -= Fader_OnFadedOut;
        GameQuitHandler.OnRequestDataSync -= GameQuitHandler_OnRequestDataSync;
    }
}
