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
    private Command keyMouse0Press;

    private Command keySpacebarRelease;
    private Command keyARelease;
    private Command keyDRelease;
    private Command keyMouse0Release;

    public void MapCommandOnPress(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebarPress = command;
        else if (key == KeyCode.A) keyAPress = command;
        else if (key == KeyCode.D) keyDPress = command;
        else if (key == KeyCode.Escape) keyEscPress = command;
        else if (key == KeyCode.Mouse0) keyMouse0Press = command;
    }

    public void MapCommandOnRelease(KeyCode key, Command command)
    {
        if (key == KeyCode.Space) keySpacebarRelease = command;
        else if (key == KeyCode.A) keyARelease = command;
        else if (key == KeyCode.D) keyDRelease = command;
        else if (key == KeyCode.Mouse0) keyMouse0Release = command;
    }

    void Start()
    {
        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        Combat combat = gameObject.GetComponent<Combat>();

        MapCommandOnPress(KeyCode.Space, new FireCommand(combat));
        MapCommandOnPress(KeyCode.A, new MoveLeftCommand(playerController));
        MapCommandOnPress(KeyCode.D, new MoveRightCommand(playerController));
        MapCommandOnPress(KeyCode.Escape, new OpenMainMenuCommand());
        MapCommandOnPress(KeyCode.Mouse0, new FireCommand(combat));

        MapCommandOnRelease(KeyCode.Space, new StopFiringCommand(combat));
        MapCommandOnRelease(KeyCode.A, new StopMovingLeftCommand(playerController));
        MapCommandOnRelease(KeyCode.D, new StopMovingRightCommand(playerController));
        MapCommandOnRelease(KeyCode.Mouse0, new StopFiringCommand(combat));
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
        if (Input.GetKey(KeyCode.Mouse0)) keyMouse0Press.Execute();

        if (Input.GetKeyDown(KeyCode.Escape)) keyEscPress.Execute();

        if (Input.GetKeyUp(KeyCode.Space)) keySpacebarRelease.Execute();
        if (Input.GetKeyUp(KeyCode.A)) keyARelease.Execute();
        if (Input.GetKeyUp(KeyCode.D)) keyDRelease.Execute();
        if (Input.GetKeyUp(KeyCode.Mouse0)) keyMouse0Release.Execute();
    }
}
