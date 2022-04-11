using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float boundaryLimit;

    private Rigidbody2D rb2;
    private Vector2 velocity;
    private Vector2 boundary;
    private float spriteWidth;
    private Vector2 direction;
    private Vector3 newPosition;
    private Vector2 newVelocity;
    private Transform myTransform;

    private void CalculateBoundary()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Vector3 screenSize = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        boundary = Camera.main.ScreenToWorldPoint(screenSize);
        boundary.x = Mathf.Clamp(boundary.x, -boundaryLimit, boundaryLimit);
        spriteWidth = spriteRenderer.sprite.bounds.size.x / 2;
    }

    void Awake()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        myTransform = transform;
    }

    void Start()
    {
        CalculateBoundary();
    }

    public void MoveLeft()
    {
        direction.x = -1;
    }

    public void MoveRight()
    {
        direction.x = 1;
    }

    public void Stop()
    {
        direction = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Move();
        SlowDown();
    }

    private void Update()
    {
        CheckOutOfBoundsX();
    }

    private void SlowDown()
    {
        if (rb2.velocity.x == 0f) return;
        rb2.velocity -= rb2.velocity / 20f;
    }

    private void CheckOutOfBoundsX()
    {
        newPosition.Set(myTransform.position.x, myTransform.position.y, myTransform.position.z);
        newPosition.x = Mathf.Clamp(newPosition.x, -boundary.x + spriteWidth, boundary.x - spriteWidth);
        myTransform.position = newPosition;
    }

    private void Move()
    {
        if (direction.x == 0 && direction.y == 0) return;
        newVelocity.Set(direction.x * speed, direction.y * speed);
        rb2.velocity = newVelocity;
    }
}
