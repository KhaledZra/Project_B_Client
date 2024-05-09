using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Project_B_Client_App.Controllers;

public static class InputController
{
    private static Dictionary<Keys, Action> _inputActions = new();
    
    public static void AddInputAction(Keys key, Action action)
    {
        _inputActions.Add(key, action);
    }
    
    public static void OnInputAction(Keys[] keys)
    {
        foreach (var key in keys)
        {
            if (_inputActions.ContainsKey(key))
            {
                _inputActions[key]();
            }
        }
    }
}