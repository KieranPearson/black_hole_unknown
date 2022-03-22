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

    private Vector3 screenSize;
    private Vector2 boundary;
    private float spriteWidth;

    void Start()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        screenSize = new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z);
        boundary = Camera.main.ScreenToWorldPoint(screenSize);
        spriteWidth = spriteRenderer.sprite.bounds.size.x / 2;
    }

    private void Move(float speed)
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        newPosition.x += speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, -boundary.x + spriteWidth, boundary.x - spriteWidth);
        transform.position = newPosition;
    }

    public void MoveLeft()
    {
        Move(-speed);
    }

    public void MoveRight()
    {
        Move(speed);
    }
}
