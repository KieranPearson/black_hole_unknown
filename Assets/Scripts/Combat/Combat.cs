using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private Vector2 fireDirection;
    [SerializeField] private int projectilePoolSize;

    private GameObject projectiles;
    private float projectileSpawnOffset;
    private float lastFired;
    private bool isFiring = false;
    private GameObject[] projectilePool;
    private int currentProjectile;

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

    private void SetupProjectilePool()
    {
        projectilePool = new GameObject[projectilePoolSize];
        for (int i = 0; i < projectilePoolSize; i++)
        {
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.name = projectilePrefab.name;
            newProjectile.SetActive(false);
            newProjectile.transform.parent = projectiles.transform;
            projectilePool[i] = newProjectile;
        }
    }

    void Awake()
    {
        projectiles = GameObject.FindWithTag("Projectiles");
        SetupProjectilePool();
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

        GameObject projectile = projectilePool[currentProjectile];
        projectile.transform.parent = projectiles.transform;
        projectile.transform.position = spawnPosition;
        projectile.SetActive(true);
        currentProjectile++;
        if (currentProjectile >= projectilePoolSize)
        {
            currentProjectile = 0;
        }
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
