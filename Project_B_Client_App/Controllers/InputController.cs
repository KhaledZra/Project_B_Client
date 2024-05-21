using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project_B_Client_App.Controllers;

public static class InputController
{ 
    private static Vector2 _direction;
    public static Vector2 Direction => _direction;
    public static bool Moving => _direction != Vector2.Zero;

    public static void Update()
    {
        _direction = Vector2.Zero;
        var keyboardState = Keyboard.GetState();

        if (keyboardState.GetPressedKeyCount() > 0)
        {
            if (keyboardState.IsKeyDown(Keys.A)) _direction.X--;
            else if (keyboardState.IsKeyDown(Keys.D)) _direction.X++;
            else if (keyboardState.IsKeyDown(Keys.W)) _direction.Y--;
            else if (keyboardState.IsKeyDown(Keys.S)) _direction.Y++;
        }
    }
}