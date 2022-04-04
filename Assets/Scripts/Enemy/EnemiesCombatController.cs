using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCombatController : MonoBehaviour
{
    public static event System.Action<GameObject> OnEnemyShoot;

    void FixedUpdate()
    {
        EnemiesShoot();
    }

    private void EnemiesShoot()
    {
        List<List<GameObject>> aliveEnemies = LevelManager.GetAliveEnemies();
        if (aliveEnemies.Count == 0) return;

        int chanceEnemyShoots = Random.Range(0, 15);
        if (chanceEnemyShoots != 0) return;
        int columnSelected = Random.Range(0, aliveEnemies.Count);
        List<GameObject> column = aliveEnemies[columnSelected];
        GameObject enemy = column[0];
        OnEnemyShoot?.Invoke(enemy);
    }
}
