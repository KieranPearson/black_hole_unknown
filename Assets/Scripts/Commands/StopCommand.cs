using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopCommand : Command
{
    private Movement movement;

    public StopCommand(Movement movement)
    {
        this.movement = movement;
    }

    public override void Execute()
    {
        movement.Stop();
    }
}
