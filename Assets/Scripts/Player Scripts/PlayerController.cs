using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(PlayerSpriteUpdater))]
public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState = new PlayerIdle();
    private Movement movement;
    private PlayerSpriteUpdater playerSpriteUpdater;

    private void Start()
    {
        movement = GetComponent<Movement>();
        playerSpriteUpdater = GetComponent<PlayerSpriteUpdater>();
    }

    public void UpdateState(Command cmd)
    {
        IPlayerState newState = currentState.Tick(movement, playerSpriteUpdater, cmd);

        if (newState != null)
        {
            currentState.Exit(movement, playerSpriteUpdater);
            currentState = newState;
            newState.Enter(movement, playerSpriteUpdater);
        }
    }
}
