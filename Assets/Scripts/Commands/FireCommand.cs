using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCommand : Command
{
    Transform transform;

    public FireCommand(Transform transform)
    {
        this.transform = transform;
    }

    public override void Execute()
    {
        Debug.Log("Fire!");
    }
}
