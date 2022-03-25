using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    private Movement movement;
    private PlayerController playerController;

    public MoveRightCommand(Movement movement)
    {
        this.movement = movement;
    }

    public MoveRightCommand(PlayerController playerController)
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
            movement.MoveRight();
        }
    }
}
