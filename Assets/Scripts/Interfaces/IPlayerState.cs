using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    public IPlayerState Tick(GameObject gameObject, string command);
    public void Enter(GameObject gameObject);
    public void Exit(GameObject gameObject);
}
