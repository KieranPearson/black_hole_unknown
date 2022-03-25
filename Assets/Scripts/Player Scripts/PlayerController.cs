using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState; // make this default the idle state

    void Start()
    {
        
    }

    private void UpdateState(string cmd)
    {
        IPlayerState newState = currentState.Tick(gameObject, cmd);
    }
}
