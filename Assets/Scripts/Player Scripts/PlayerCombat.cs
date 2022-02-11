using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerCombat : MonoBehaviour
{
    [SerializeField] float FIRE_RATE;

    private PlayerInput playerInput;
    private float lastFired;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        FireProjectile();
    }

    private void FireProjectile()
    {
        if (!playerInput.IsFireDown()) return;
        float fireTime = Time.time;
        if ((fireTime - lastFired) < FIRE_RATE) return;
        lastFired = fireTime;
        CreateProjectile();
    }

    private void CreateProjectile()
    {
        Debug.Log("FIRE!!!");
    }
}
