using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Project_B_Client_App.Controllers;

public static class InputController
{
    private static Dictionary<Keys, List<Action>> _inputActions = new();
    
    public static void AddInputAction(Keys key, List<Action> actions)
    {
        _inputActions.Add(key, actions);
    }
    
    public static void OnInputAction(Keys[] keys)
    {
        foreach (var key in keys)
        {
            if (_inputActions.ContainsKey(key))
            {
                // Run all stored actions for the key
                _inputActions[key].ForEach(action => action());
            }
        }
    }
}