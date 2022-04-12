using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievements : MonoBehaviour
{
    public static Achievements instance { get; private set; }

    [SerializeField] private Achievement[] achievements;

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

    public Achievement[] GetAchievements()
    {
        return achievements;
    }
}
