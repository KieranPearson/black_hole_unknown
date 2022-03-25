using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingLeftCommand : Command
{
    private Movement movement;
    private PlayerController playerController;

    public StopMovingLeftCommand(Movement movement)
    {
        this.movement = movement;
    }

    public StopMovingLeftCommand(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public override void Execute()
    {
        if (playerController)
        {
            playerController.UpdateState(this);
        }
        else
        {
            movement.Stop();
        }
    }
}
