using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Combat))]
public class EnemyCombatHandler : MonoBehaviour
{
    private Combat combat;

    private void EnemiesCombatController_OnEnemyShoot(GameObject enemy)
    {
        if (enemy != gameObject) return;
        combat.FireOnce();
    }

    void OnEnable()
    {
        EnemiesCombatController.OnEnemyShoot += EnemiesCombatController_OnEnemyShoot;
    }

    void OnDisable()
    {
        EnemiesCombatController.OnEnemyShoot -= EnemiesCombatController_OnEnemyShoot;
    }

    void Awake()
    {
        combat = GetComponent<Combat>();
    }
}
