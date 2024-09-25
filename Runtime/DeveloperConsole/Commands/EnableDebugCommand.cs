using PlazmaGames.Console;
using PlazmaGames.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnableDebugCommand", menuName = "Console Commands/Debug/Enable")]
public class EnableDebugCommand : ConsoleCommand
{
    public override bool Process(string[] args, out ConsoleResponse msg)
    {
        if (args.Length == 0)
        {
            msg = new($"Need to specify an state (0 or 1).", ResponseType.Warning);
            return false;
        }

        if (int.TryParse(args[0], out int mode))
        {
            GameManager.InDebugMode = mode > 0;
        }
        else 
        {
            msg = new($"Argument must be an of type {typeof(int)}.", ResponseType.Warning);
            return false;
        }

        msg = new(ResponseType.None);
        return true;
    }
}
