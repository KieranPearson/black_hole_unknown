using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFiringCommand : Command
{
    Transform transform;

    public StopFiringCommand(Transform transform)
    {
        this.transform = transform;
    }

    public override void Execute()
    {
        Debug.Log("Stop firing!");
    }
}
