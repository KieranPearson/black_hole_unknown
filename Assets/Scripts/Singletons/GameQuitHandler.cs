using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuitHandler : MonoBehaviour
{
    public static GameQuitHandler instance { get; private set; }

    public static event System.Action OnRequestDataSync;
    public static event System.Action OnRequestDataSave;

    private static bool isDataSynced = false;
    private static bool isDataSaved = false;

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

    public static void DataSynced()
    {
        isDataSynced = true;
    }

    public static void DataSaved()
    {
        isDataSaved = true;
    }

    IEnumerator ForceSave()
    {
        while (!isDataSynced)
        {
            yield return new WaitForSeconds(0.1f);
        }
        OnRequestDataSave?.Invoke();
        while (!isDataSaved)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    static bool WantsToQuit()
    {
        OnRequestDataSync?.Invoke();
        instance.StartCoroutine("ForceSave");
        return true;
    }

    private void Start()
    {
        Application.wantsToQuit += WantsToQuit;
    }
}
