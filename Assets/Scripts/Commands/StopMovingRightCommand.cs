using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingRightCommand : Command
{
    private Movement movement;
    private PlayerController playerController;

    public StopMovingRightCommand(Movement movement)
    {
        this.movement = movement;
    }

    public StopMovingRightCommand(PlayerController playerController)
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
