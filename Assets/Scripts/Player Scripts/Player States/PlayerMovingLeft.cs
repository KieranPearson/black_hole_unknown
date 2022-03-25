using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingLeft : IPlayerState
{
    public void Enter(Movement movement)
    {
        movement.MoveLeft();
    }

    public IPlayerState Tick(Movement movement, Command cmd)
    {
        if (cmd.GetType().Name == "StopMovingLeftCommand") return new PlayerIdle();
        else if (cmd.GetType().Name == "MoveRightCommand") return new PlayerMovingRight();
        return null;
    }

    public void Exit(Movement movement)
    {
        
    }
}
