using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftCommand : Command
{
    private Movement movement;

    public MoveLeftCommand(Movement movement)
    {
        this.movement = movement;
    }

    public override void Execute()
    {
        movement.MoveLeft();
    }
}
