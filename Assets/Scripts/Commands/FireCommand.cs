using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : Command
{
    Combat combat;

    public FireCommand(Combat combat)
    {
        this.combat = combat;
    }

    public override void Execute()
    {
        combat.ToggleFire(true);
    }
}
