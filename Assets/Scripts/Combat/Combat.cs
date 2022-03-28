using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private Vector2 fireDirection;

    private GameObject projectiles;
    private float projectileSpawnOffset;
    private float lastFired;

    private bool isFiring = false;

    private void CalculateSpawnOffset()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (projectilePrefab == null) return;
        if (projectilePrefab.GetComponent<ProjectileMovement>().getMovesDown())
        {
            projectileSpawnOffset = -(spriteRenderer.sprite.bounds.size.y / 2) - 0.6f;
        } else
        {
            projectileSpawnOffset = (spriteRenderer.sprite.bounds.size.y / 2) + 0.6f;
        }
    }

    void Awake()
    {
        projectiles = GameObject.FindWithTag("Projectiles");
    }

    void Start()
    {
        CalculateSpawnOffset();
    }

    void FixedUpdate()
    {
        Fire();
    }

    private void Fire()
    {
        if (!isFiring) return;
        FireOnce();
    }

    private void SpawnProjectile()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += projectileSpawnOffset;
        projectilePrefab.transform.position = spawnPosition;
        GameObject newProjectile = Instantiate(projectilePrefab);
        newProjectile.transform.parent = projectiles.transform;
    }

    public void FireOnce()
    {
        float fireTime = Time.time;
        if ((fireTime - lastFired) < fireRate) return;
        lastFired = fireTime;
        SpawnProjectile();
    }

    public void ToggleFire(bool toggle)
    {
        isFiring = toggle;
    }
}
