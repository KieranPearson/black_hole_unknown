using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingLeftCommand : Command
{
    private Movement movement;

    public StopMovingLeftCommand(Movement movement)
    {
        this.movement = movement;
    }

    public override void Execute()
    {
        movement.Stop(new Vector2(-1, 0));
    }
}
