using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightCommand : Command
{
    private Movement movement;

    public MoveRightCommand(Movement movement)
    {
        this.movement = movement;
    }

    public override void Execute()
    {
        movement.MoveRight();
    }
}
