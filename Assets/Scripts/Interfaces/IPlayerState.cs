using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public IPlayerState Tick(Movement movement, Command cmd);
    public void Enter(Movement movement);
    public void Exit(Movement movement);
}
