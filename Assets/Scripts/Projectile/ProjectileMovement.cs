using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    Rigidbody2D rb2;
    private float currentSpeed;
    private Vector2 currentVelocity;

    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(float newSpeed)
    {
        this.currentSpeed = newSpeed;
        currentVelocity.y = currentSpeed;
    }

    void FixedUpdate()
    {
        rb2.velocity = currentVelocity;
    }
}
