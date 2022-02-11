using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float MAX_LEFT_POS;
    [SerializeField] float MAX_RIGHT_POS;
    [SerializeField] float SPEED;
    [SerializeField] float STILL_SPEED;

    private Transform thisTransform;
    private Rigidbody2D rb2;
    private PlayerInput playerInput;
    private Vector2 velocity;
    private bool movingPlayer;

    private void Awake()
    {
        thisTransform = transform;
        rb2 = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        movingPlayer = false;
    }

    void Update()
    {
        ControlPlayer();
    }

    private void StopPlayer()
    {
        if (!movingPlayer) return;
        velocity.x = STILL_SPEED;
        rb2.velocity = velocity;
        movingPlayer = false;
    }

    private void MovePlayer(string direction)
    {
        if (direction == "Left")
        {
            if ((thisTransform.position.x - SPEED) < (-MAX_LEFT_POS))
            {
                StopPlayer();
                return;
            }
            velocity.x = -SPEED;
        }
        else
        {
            if ((thisTransform.position.x + SPEED) > MAX_RIGHT_POS)
            {
                StopPlayer();
                return;
            }
            velocity.x = SPEED;
        }
        rb2.velocity = velocity;
        movingPlayer = true;
    }

    private void ControlPlayer()
    {
        velocity = rb2.velocity;

        if (playerInput.IsMoveLeftDown() && playerInput.IsMoveRightDown())
        {
            StopPlayer();
        }
        else if (playerInput.IsMoveLeftDown())
        {
            MovePlayer("Left");
        }
        else if (playerInput.IsMoveRightDown())
        {
            MovePlayer("Right");
        }
        else
        {
            StopPlayer();
        } 
    }
}
