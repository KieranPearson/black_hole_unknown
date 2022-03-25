using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState = new PlayerIdle();
    private Movement movement;

    private void Start()
    {
        movement = GetComponent<Movement>();
    }

    public void UpdateState(Command cmd)
    {
        IPlayerState newState = currentState.Tick(movement, cmd);

        if (newState != null)
        {
            currentState.Exit(movement);
            currentState = newState;
            newState.Enter(movement);
        }
    }
}
