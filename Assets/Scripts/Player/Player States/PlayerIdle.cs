using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdle : IPlayerState
{
    public void Enter(Movement movement, PlayerSpriteUpdater playerSpriteUpdater)
    {
        movement.Stop();
        playerSpriteUpdater.SetIdleSprite();
    }

    public IPlayerState Tick(Movement movement, PlayerSpriteUpdater playerSpriteUpdater,
                             Command cmd)
    {
        if (cmd.GetType().Name == "MoveLeftCommand") return new PlayerMovingLeft();
        else if (cmd.GetType().Name == "MoveRightCommand") return new PlayerMovingRight();
        return null;
    }

    public void Exit(Movement movement, PlayerSpriteUpdater playerSpriteUpdater)
    {

    }
}
