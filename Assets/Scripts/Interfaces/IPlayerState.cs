using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public IPlayerState Tick(Movement movement, PlayerSpriteUpdater playerSpriteUpdater, 
                             Command cmd);
    public void Enter(Movement movement, PlayerSpriteUpdater playerSpriteUpdater);
    public void Exit(Movement movement, PlayerSpriteUpdater playerSpriteUpdater);
}
