using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float MIN_X_POS = -3;
    [SerializeField] float MAX_X_POS = 3;

    private Transform thisTransform;
    private Rigidbody2D rb2;
    private PlayerInput playerInput;

    private void Awake()
    {
        thisTransform = transform;
        rb2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 velocity = rb2.velocity;

        if (playerInput.IsMoveLeftDown())
        {
            velocity.x = -10.0f;
            rb2.velocity = velocity;
        }
        else if (playerInput.IsMoveRightDown())
        {
            velocity.x = 10.0f;
            rb2.velocity = velocity;
        }
        else
        {
            velocity.x = 0f;
            rb2.velocity = velocity;
        } 
    }
}
