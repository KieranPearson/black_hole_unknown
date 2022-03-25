using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private float projectileSpawnOffset;

    private SpriteRenderer spriteRenderer;

    private void CalculateSpawnOffset()
    {
        if (projectilePrefab == null) return;
        if (projectilePrefab.GetComponent<Rigidbody2D>().gravityScale > 0)
        {
            projectileSpawnOffset = -(spriteRenderer.sprite.bounds.size.x / 2) - 0.5f;
        } else
        {
            projectileSpawnOffset = (spriteRenderer.sprite.bounds.size.x / 2) + 0.5f;
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        CalculateSpawnOffset();
    }

    public void Fire()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += projectileSpawnOffset;
        projectilePrefab.transform.position = spawnPosition;
        Instantiate(projectilePrefab);
    }
}
