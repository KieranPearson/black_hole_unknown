using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(AudioSource))]
public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private int projectilePoolSize;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private bool projectilesMoveDown;
    [SerializeField] private AudioClip fireSound;

    public static event System.Action<AudioSource> OnPlayFireSound;

    private GameObject projectiles;
    private float projectileSpawnOffset;
    private float lastFired;
    private bool isFiring = false;
    private GameObject[] projectilePool;
    private ProjectileMovement[] projectilesMovement;
    private int currentProjectile;
    private float currentFireRate;
    private float currentProjectileSpeed;
    private AudioSource audioSource;

    private void CalculateSpawnOffset()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (projectilePrefab == null) return;
        if (projectilesMoveDown)
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
        projectilesMovement = new ProjectileMovement[projectilePoolSize];
        for (int i = 0; i < projectilePoolSize; i++)
        {
            GameObject newProjectile = Instantiate(projectilePrefab);
            newProjectile.SetActive(false);
            newProjectile.name = projectilePrefab.name;
            newProjectile.transform.parent = projectiles.transform;
            projectilePool[i] = newProjectile;
            projectilesMovement[i] = newProjectile.GetComponent<ProjectileMovement>();
        }
    }

    void Awake()
    {
        projectiles = GameObject.FindWithTag("Projectiles");
        currentFireRate = fireRate;
        if (projectilesMoveDown)
        {
            projectileSpeed = -projectileSpeed;
        }
        currentProjectileSpeed = projectileSpeed;
        SetupProjectilePool();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Start()
    {
        CalculateSpawnOffset();
        audioSource.clip = fireSound;
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

    private void UseProjectile()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += projectileSpawnOffset;

        GameObject projectile = projectilePool[currentProjectile];
        projectile.transform.position = spawnPosition;
        projectile.SetActive(true);
        projectilesMovement[currentProjectile].SetSpeed(currentProjectileSpeed);
        currentProjectile++;
        if (currentProjectile >= projectilePoolSize)
        {
            currentProjectile = 0;
        }
        OnPlayFireSound?.Invoke(audioSource);
    }

    public void FireOnce()
    {
        float fireTime = Time.time;
        if ((fireTime - lastFired) < currentFireRate) return;
        lastFired = fireTime;
        UseProjectile();
    }

    public void ToggleFire(bool toggle)
    {
        isFiring = toggle;
    }

    public void SetFireRate(float fireRate)
    {
        this.currentFireRate = fireRate;
    }

    public void SetDefaultFireRate()
    {
        this.currentFireRate = fireRate;
    }

    public void SetDefaultFireRate(float rate)
    {
        fireRate = rate;
    }

    public void SetProjectilesSpeed(float newSpeed)
    {
        if (projectilesMoveDown) newSpeed = -newSpeed;
        currentProjectileSpeed = newSpeed;
        for (int i = 0; i < projectilePoolSize; i++)
        {
            projectilesMovement[i].SetSpeed(newSpeed);
        }
    }

    public void SetProjectilesDefaultSpeed()
    {
        currentProjectileSpeed = projectileSpeed;
        for (int i = 0; i < projectilePoolSize; i++)
        {
            projectilesMovement[i].SetSpeed(projectileSpeed);
        }
    }

    public float GetProjectilesDefaultSpeed()
    {
        return projectileSpeed;
    }

    public bool ProjectilesDefaultMoveDown()
    {
        return projectilesMoveDown;
    }
}
