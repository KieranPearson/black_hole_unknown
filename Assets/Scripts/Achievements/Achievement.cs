using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAchievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;

    void Start()
    {

    }
}
