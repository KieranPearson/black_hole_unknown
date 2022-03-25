using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    private Command keySpacebarPress;
    private Command keyAPress;
    private Command keyDPress;

    private Command keySpacebarRelease;
    private Command keyARelease;
    private Command keyDRelease;

    public void MapCommandOnPress(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebarPress = command;
        else if (key == KeyCode.A) keyAPress = command;
        else if (key == KeyCode.D) keyDPress = command;
    }

    public void MapCommandOnRelease(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebarRelease = command;
        else if (key == KeyCode.A) keyARelease = command;
        else if (key == KeyCode.D) keyDRelease = command;
    }

    void Start()
    {
        Movement movement = gameObject.GetComponent<Movement>();

        MapCommandOnPress(KeyCode.Space, new FireCommand(transform));
        MapCommandOnPress(KeyCode.A, new MoveLeftCommand(movement));
        MapCommandOnPress(KeyCode.D, new MoveRightCommand(movement));

        MapCommandOnRelease(KeyCode.Space, new StopFiringCommand(transform));
        MapCommandOnRelease(KeyCode.A, new StopMovingLeftCommand(movement));
        MapCommandOnRelease(KeyCode.D, new StopMovingRightCommand(movement));
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) keySpacebarPress.Execute();
        if (Input.GetKeyDown(KeyCode.A)) keyAPress.Execute();
        if (Input.GetKeyDown(KeyCode.D)) keyDPress.Execute();

        if (Input.GetKeyUp(KeyCode.Space)) keySpacebarRelease.Execute();
        if (Input.GetKeyUp(KeyCode.A)) keyARelease.Execute();
        if (Input.GetKeyUp(KeyCode.D)) keyDRelease.Execute();
    }
}
