using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb2;
    private Vector2 velocity;
    private SpriteRenderer spriteRenderer;

    private Vector2 boundary;
    private float spriteWidth;

    private Vector2 direction;

    void Start()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Vector3 screenSize = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        boundary = Camera.main.ScreenToWorldPoint(screenSize);
        spriteWidth = spriteRenderer.sprite.bounds.size.x / 2;
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
        if (rb2.velocity.x != 0)
        {
            rb2.velocity -= rb2.velocity / 20f;
        }
    }

    private void CheckOutOfBoundsX()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        newPosition.x = Mathf.Clamp(newPosition.x, -boundary.x + spriteWidth, boundary.x - spriteWidth);
        transform.position = newPosition;
    }

    private void Move()
    {
        if (direction.x == 0 && direction.y == 0) return;
        rb2.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }
}
