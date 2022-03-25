using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovingRightCommand : Command
{
    private Movement movement;

    public StopMovingRightCommand(Movement movement)
    {
        this.movement = movement;
    }

    public override void Execute()
    {
        movement.Stop(new Vector2(1, 0));
    }
}
