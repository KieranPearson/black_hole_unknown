using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    Rigidbody2D rb2;
    private float currentSpeed;

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2.velocity = new Vector2(0, currentSpeed);
    }

    public void SetSpeed(float newSpeed)
    {
        this.currentSpeed = newSpeed;
    }
}
