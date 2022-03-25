using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    Rigidbody2D rb2;

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb2.velocity = new Vector2(0, speed);
    }
}
