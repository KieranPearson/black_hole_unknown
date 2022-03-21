using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerInput : MonoBehaviour
{
    private Command keySpacebar;
    private Command keyA;
    private Command keyD;

    // Non-mappable commands
    private Command stop;

    public void MapCommand(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebar = command;
        else if (key == KeyCode.A) keyA = command;
        else if (key == KeyCode.D) keyD = command;
    }

    void Start()
    {
        Movement movement = gameObject.GetComponent<Movement>();

        MapCommand(KeyCode.Space, new FireCommand(transform));
        MapCommand(KeyCode.A, new MoveLeftCommand(movement));
        MapCommand(KeyCode.D, new MoveRightCommand(movement));

        stop = new StopCommand(movement);
    }

    void FixedUpdate()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space)) keySpacebar.Execute();
        if (Input.GetKey(KeyCode.A)) keyA.Execute();
        else if (Input.GetKey(KeyCode.D)) keyD.Execute();
        else stop.Execute();
    }
}
