using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemiesMovement : MonoBehaviour
{
    [SerializeField] private float moveDownAmount;

    public static event System.Action OnEnemiesMovedDown;

    private float speed;
    private float currentSpeed;
    private Rigidbody2D rb2;

    private void EnemyEdgeDetector_OnEnemyHitEdge(bool isLeft)
    {
        if (isLeft && currentSpeed < 0)
        {
            currentSpeed = speed;
            MoveDown();
        } else if (!isLeft && currentSpeed > 0)
        {
            currentSpeed = -speed;
            MoveDown();
        }
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void SetCurrentSpeed(float currentSpeed)
    {
        this.currentSpeed = currentSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        float newCurrentSpeed = newSpeed;
        if (currentSpeed < 0)
        {
            newCurrentSpeed = -newSpeed;
        }
        speed = newSpeed;
        currentSpeed = newCurrentSpeed;
    }

    private void MoveDown()
    {
        Vector3 position = transform.position;
        position.y = position.y - moveDownAmount;
        transform.position = position;
        OnEnemiesMovedDown?.Invoke();
    }

    void OnEnable()
    {
        EnemyEdgeDetector.OnEnemyHitEdge += EnemyEdgeDetector_OnEnemyHitEdge;
    }

    void OnDisable()
    {
        EnemyEdgeDetector.OnEnemyHitEdge -= EnemyEdgeDetector_OnEnemyHitEdge;
    }

    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb2.velocity = new Vector2(currentSpeed, 0f);
    }
}
