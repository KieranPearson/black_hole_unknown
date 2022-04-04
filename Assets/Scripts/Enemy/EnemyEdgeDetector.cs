using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyEdgeDetector : MonoBehaviour
{
    [SerializeField] private float boundaryLimit;

    private Vector2 boundary;
    private float spriteWidth;

    public static event System.Action<bool> OnEnemyHitEdge;

    private void CalculateBoundary()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        boundary = Camera.main.ScreenToWorldPoint(screenSize);
        boundary.x = Mathf.Clamp(boundary.x, -boundaryLimit, boundaryLimit);
        spriteWidth = spriteRenderer.sprite.bounds.size.x / 2;
    }

    void Start()
    {
        CalculateBoundary();
    }

    void FixedUpdate()
    {
        CheckForEdgeCollision();
    }

    private void CheckForEdgeCollision()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        if (newPosition.x <= (-boundary.x + spriteWidth))
        {
            OnEnemyHitEdge?.Invoke(true);
        } else if (newPosition.x >= (boundary.x - spriteWidth))
        {
            OnEnemyHitEdge?.Invoke(false);
        }
    }
}
