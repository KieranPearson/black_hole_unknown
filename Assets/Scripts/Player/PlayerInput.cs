using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Combat))]
public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool isPlayer2Input;

    private List<(KeyCode, Command)> keyPressMap;
    private List<(KeyCode, Command)> keyReleaseMap;
    private List<(KeyCode, Command)> keyHeldMap;

    public void MapKeyPress(KeyCode key, Command command)
    {
        keyPressMap.Add((key, command));
    }

    public void MapKeyRelease(KeyCode key, Command command)
    {
        keyReleaseMap.Add((key, command));
    }

    public void MapKeyHeld(KeyCode key, Command command)
    {
        keyHeldMap.Add((key, command));
    }

    private void Awake()
    {
        keyPressMap = new List<(KeyCode, Command)>();
        keyReleaseMap = new List<(KeyCode, Command)>();
        keyHeldMap = new List<(KeyCode, Command)>();
    }

    private void Start()
    {
        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        Combat combat = gameObject.GetComponent<Combat>();

        if (isPlayer2Input)
        {
            MapKeyHeld(KeyCode.Insert, new FireCommand(combat));
            MapKeyHeld(KeyCode.RightShift, new FireCommand(combat));
            MapKeyHeld(KeyCode.UpArrow, new FireCommand(combat));
            MapKeyHeld(KeyCode.LeftArrow, new MoveLeftCommand(playerController));
            MapKeyHeld(KeyCode.RightArrow, new MoveRightCommand(playerController));

            MapKeyRelease(KeyCode.Insert, new StopFiringCommand(combat));
            MapKeyRelease(KeyCode.RightShift, new StopFiringCommand(combat));
            MapKeyRelease(KeyCode.UpArrow, new StopFiringCommand(combat));
            MapKeyRelease(KeyCode.LeftArrow, new StopMovingLeftCommand(playerController));
            MapKeyRelease(KeyCode.RightArrow, new StopMovingRightCommand(playerController));
        }
        else
        {
            MapKeyPress(KeyCode.Escape, new OpenMainMenuCommand());

            MapKeyHeld(KeyCode.Space, new FireCommand(combat));
            MapKeyHeld(KeyCode.A, new MoveLeftCommand(playerController));
            MapKeyHeld(KeyCode.D, new MoveRightCommand(playerController));

            MapKeyRelease(KeyCode.Space, new StopFiringCommand(combat));
            MapKeyRelease(KeyCode.A, new StopMovingLeftCommand(playerController));
            MapKeyRelease(KeyCode.D, new StopMovingRightCommand(playerController));
        }
    }

    private void Update()
    {
        HandleKeysPressed();
        HandleKeysReleased();
        HandleKeysHeld();
    }

    private void HandleKeysPressed()
    {
        for (int i = 0; i < keyPressMap.Count; i++)
        {
            if (!Input.GetKeyDown(keyPressMap[i].Item1)) continue;
            keyPressMap[i].Item2.Execute();
        }
    }

    private void HandleKeysReleased()
    {
        for (int i = 0; i < keyReleaseMap.Count; i++)
        {
            if (!Input.GetKeyUp(keyReleaseMap[i].Item1)) continue;
            keyReleaseMap[i].Item2.Execute();
        }
    }

    private void HandleKeysHeld()
    {
        for (int i = 0; i < keyHeldMap.Count; i++)
        {
            if (!Input.GetKey(keyHeldMap[i].Item1)) continue;
            keyHeldMap[i].Item2.Execute();
        }
    }
}
