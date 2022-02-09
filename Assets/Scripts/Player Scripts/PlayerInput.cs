using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] KeyCode moveLeft;
    [SerializeField] KeyCode moveRight;

    private bool isMoveLeftDown;
    private bool isMoveRightDown;

    private void Awake()
    {
        isMoveLeftDown = false;
        isMoveRightDown = false;
    }

    private void Update()
    {
        CheckMovement();
    }

    private void CheckMovement()
    {

    }
}
