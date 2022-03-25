using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IPlayerState
{
    public void Enter(Movement movement)
    {
        movement.Stop();
    }

    public IPlayerState Tick(Movement movement, Command cmd)
    {
        if (cmd.GetType().Name == "MoveLeftCommand") return new PlayerMovingLeft();
        else if (cmd.GetType().Name == "MoveRightCommand") return new PlayerMovingRight();
        return null;
    }

    public void Exit(Movement movement)
    {

    }
}
