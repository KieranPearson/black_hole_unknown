using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFiringCommand : Command
{
    Combat combat;

    public StopFiringCommand(Combat combat)
    {
        this.combat = combat;
    }

    public override void Execute()
    {
        combat.ToggleFire(false);
    }
}
