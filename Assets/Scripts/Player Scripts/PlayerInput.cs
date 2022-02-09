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
        CheckMovementInput();
    }

    private void CheckMovementInput()
    {
        if (Input.GetKeyDown(moveLeft))
        {
            isMoveLeftDown = true;
        }
        else if (Input.GetKeyUp(moveLeft))
        {
            isMoveLeftDown = false;
        }
        if (Input.GetKeyDown(moveRight))
        {
            isMoveRightDown = true;
        }
        else if (Input.GetKeyUp(moveRight))
        {
            isMoveRightDown = false;
        }
    }

    public bool IsMoveLeftDown() => isMoveLeftDown;
    public bool IsMoveRightDown() => isMoveRightDown;
}
