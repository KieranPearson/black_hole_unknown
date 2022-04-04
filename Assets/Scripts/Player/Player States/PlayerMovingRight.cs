using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovingRight : IPlayerState
{
    public void Enter(Movement movement, PlayerSpriteUpdater playerSpriteUpdater)
    {
        movement.MoveRight();
        playerSpriteUpdater.SetStrafeRightSprite();
    }

    public IPlayerState Tick(Movement movement, PlayerSpriteUpdater playerSpriteUpdater,
                             Command cmd)
    {
        if (cmd.GetType().Name == "StopMovingRightCommand") return new PlayerIdle();
        else if (cmd.GetType().Name == "MoveLeftCommand") return new PlayerMovingLeft();
        return null;
    }

    public void Exit(Movement movement, PlayerSpriteUpdater playerSpriteUpdater)
    {
        
    }
}
