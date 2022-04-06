using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMainMenuCommand : Command
{
    public static event System.Action OnExitingLevel;

    public override void Execute()
    {
        OnExitingLevel?.Invoke();
    }
}
