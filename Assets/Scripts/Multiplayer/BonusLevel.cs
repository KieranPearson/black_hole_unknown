using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBonusLevel", menuName = "BonusLevel")]
public class BonusLevel : ScriptableObject
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerFireRate;

    [SerializeField] private float enemySpeed;
    [SerializeField] private float enemyMoveDownAmount;
    [SerializeField] private float enemySpawnDistance;
    [SerializeField] private int enemyFireChance;
    [SerializeField] private float enemyFireRate;

    [SerializeField] private bool asteroidsInvincibile;

    void Start()
    {

    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public float GetPlayerFireRate()
    {
        return playerFireRate;
    }

    public float GetEnemySpeed()
    {
        return enemySpeed;
    }

    public float GetEnemyMoveDownAmount()
    {
        return enemyMoveDownAmount;
    }

    public float GetEnemySpawnDistance()
    {
        return enemySpawnDistance;
    }

    public int GetEnemyFireChance()
    {
        return enemyFireChance;
    }

    public float GetEnemyFireRate()
    {
        return enemyFireRate;
    }

    public bool AsteroidsInvincibile()
    {
        return asteroidsInvincibile;
    }
}
