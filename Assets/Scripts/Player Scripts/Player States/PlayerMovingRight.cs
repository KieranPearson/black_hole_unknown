using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingRight : IPlayerState
{
    public void Enter(Movement movement)
    {
        movement.MoveRight();
    }

    public IPlayerState Tick(Movement movement, Command cmd)
    {
        if (cmd.GetType().Name == "StopMovingRightCommand") return new PlayerIdle();
        else if (cmd.GetType().Name == "MoveLeftCommand") return new PlayerMovingLeft();
        return null;
    }

    public void Exit(Movement movement)
    {
        
    }
}
