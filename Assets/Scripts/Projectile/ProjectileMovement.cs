using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private bool movesDown;

    Rigidbody2D rb2;

    public bool getMovesDown()
    {
        return movesDown;
    }

    void Start()
    {
        rb2 = GetComponent<Rigidbody2D>();
        if (movesDown)
        {
            speed = -speed;
        }
    }

    void FixedUpdate()
    {
        rb2.velocity = new Vector2(0, speed);
    }
}
