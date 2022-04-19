using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevels : MonoBehaviour
{
    public static BonusLevels instance { get; private set; }

    [SerializeField] private BonusLevel[] bonusLevels;

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
    }

    public BonusLevel[] GetBonusLevels()
    {
        return bonusLevels;
    }
}
