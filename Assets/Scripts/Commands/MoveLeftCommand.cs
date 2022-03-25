using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : Command
{
    private Movement movement;
    private PlayerController playerController;

    public MoveLeftCommand(Movement movement)
    {
        this.movement = movement;
    }

    public MoveLeftCommand(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public override void Execute()
    {
        if (playerController)
        {
            playerController.UpdateState(this);
        } else
        {
            movement.MoveLeft();
        }
    }
}
