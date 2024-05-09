using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Project_B_Client_App.Controllers;

// The controller for the game, mostly to reduce clutter in the main class and to keep the game logic separate
public static class GameController
{
    public static GameTime GameTime { get; private set; }
    
    public static void Update(GameTime gameTime)
    {
        GameTime = gameTime;
    }
    public static void InitializeGameInputs(Action exitGame)
    {
        // Initialize the game inputs
        // Movement
        InputController.AddInputAction(Keys.W, PlayerController.MovePlayerForward);
        InputController.AddInputAction(Keys.S, PlayerController.MovePlayerBackward);
        InputController.AddInputAction(Keys.D, PlayerController.MovePlayerRight);
        InputController.AddInputAction(Keys.A, PlayerController.MovePlayerLeft);
        
        // Exit
        InputController.AddInputAction(Keys.Escape, exitGame);
    }
}