using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] KeyCode moveLeft;
    [SerializeField] KeyCode moveRight;
    [SerializeField] KeyCode fire;

    private bool isMoveLeftDown;
    private bool isMoveRightDown;
    private bool isFireDown;

    private void Awake()
    {
        isMoveLeftDown = false;
        isMoveRightDown = false;
    }

    private void Update()
    {
        CheckMovementInput();
        CheckFireInput();
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

    private void CheckFireInput()
    {
        if (Input.GetKeyDown(fire))
        {
            isFireDown = true;
        }
        else if (Input.GetKeyUp(fire))
        {
            isFireDown = false;
        }
    }

    public bool IsMoveLeftDown() => isMoveLeftDown;
    public bool IsMoveRightDown() => isMoveRightDown;
    public bool IsFireDown() => isFireDown;
}
