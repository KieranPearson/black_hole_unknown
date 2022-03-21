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

    void Start()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Move(float speed)
    {
        velocity = rb2.velocity;
        if (velocity.x == speed) return;
        velocity.x = speed;
        rb2.velocity = velocity;
    }

    public void Stop()
    {
        velocity = rb2.velocity;
        if (velocity.x == 0.0f) return;
        velocity.x = 0.0f;
        rb2.velocity = velocity;
    }

    public void MoveLeft()
    {
        Vector3 targetPos = new Vector3(transform.position.x - speed,
                                        transform.position.y,
                                        transform.position.z);
        targetPos.x -= spriteRenderer.sprite.bounds.size.x / 2;
        Vector3 posOnScreen = Camera.main.WorldToViewportPoint(targetPos);
        if (posOnScreen.x < 0f)
        {
            Stop();
            return;
        }
        Move(-speed);
    }

    public void MoveRight()
    {
        Vector3 targetPos = new Vector3(transform.position.x + speed,
                                        transform.position.y, 
                                        transform.position.z);
        targetPos.x += spriteRenderer.sprite.bounds.size.x / 2;
        Vector3 posOnScreen = Camera.main.WorldToViewportPoint(targetPos);
        if (posOnScreen.x > 1.0f)
        {
            Stop();
            return;
        }
        Move(speed);
    }
}
