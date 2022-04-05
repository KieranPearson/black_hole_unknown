using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Combat))]
public class PlayerInput : MonoBehaviour
{
    private Command keySpacebarPress;
    private Command keyAPress;
    private Command keyDPress;
    private Command keyEscPress;

    private Command keySpacebarRelease;
    private Command keyARelease;
    private Command keyDRelease;

    public void MapCommandOnPress(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebarPress = command;
        else if (key == KeyCode.A) keyAPress = command;
        else if (key == KeyCode.D) keyDPress = command;
        else if (key == KeyCode.Escape) keyEscPress = command;
    }

    public void MapCommandOnRelease(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebarRelease = command;
        else if (key == KeyCode.A) keyARelease = command;
        else if (key == KeyCode.D) keyDRelease = command;
    }

    void Start()
    {
        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        Combat combat = gameObject.GetComponent<Combat>();

        MapCommandOnPress(KeyCode.Space, new FireCommand(combat));
        MapCommandOnPress(KeyCode.A, new MoveLeftCommand(playerController));
        MapCommandOnPress(KeyCode.D, new MoveRightCommand(playerController));
        MapCommandOnPress(KeyCode.Escape, new OpenMainMenuCommand());

        MapCommandOnRelease(KeyCode.Space, new StopFiringCommand(combat));
        MapCommandOnRelease(KeyCode.A, new StopMovingLeftCommand(playerController));
        MapCommandOnRelease(KeyCode.D, new StopMovingRightCommand(playerController));
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space)) keySpacebarPress.Execute();
        if (Input.GetKey(KeyCode.A)) keyAPress.Execute();
        if (Input.GetKey(KeyCode.D)) keyDPress.Execute();

        if (Input.GetKeyDown(KeyCode.Escape)) keyEscPress.Execute();

        if (Input.GetKeyUp(KeyCode.Space)) keySpacebarRelease.Execute();
        if (Input.GetKeyUp(KeyCode.A)) keyARelease.Execute();
        if (Input.GetKeyUp(KeyCode.D)) keyDRelease.Execute();
    }
}
