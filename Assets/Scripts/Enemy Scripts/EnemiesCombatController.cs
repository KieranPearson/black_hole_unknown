using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesCombatController : MonoBehaviour
{
    [SerializeField] private float shootRate;
    [SerializeField] private int maxEnemiesShootAtOnce;

    public static event System.Action<GameObject> OnEnemyShoot;

    private float lastShootTime;

    void FixedUpdate()
    {
        EnemiesShoot();
    }

    private void EnemiesShoot()
    {
        List<List<GameObject>> aliveEnemies = LevelManager.GetAliveEnemies();
        if (aliveEnemies.Count == 0) return;
        if ((Time.time - lastShootTime) < shootRate) return;

        int enemiesToShoot = Random.Range(1, maxEnemiesShootAtOnce);
        for (int i = 0; i < enemiesToShoot; i++)
        {
            int columnSelected = Random.Range(0, aliveEnemies.Count);
            List<GameObject> column = aliveEnemies[columnSelected];
            GameObject enemy = column[0];
            OnEnemyShoot?.Invoke(enemy);
        }
        lastShootTime = Time.time;
    }
}
