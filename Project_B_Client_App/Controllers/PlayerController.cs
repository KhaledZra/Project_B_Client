using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.GameObjects;

namespace Project_B_Client_App.Controllers;

public static class PlayerController
{
    private static Player _player;
    
    public static void InitializePlayer(ContentManager contentManager, Vector2 position2D, String assetName)
    {
        _player = new(
            contentManager.Load<Texture2D>(assetName),
            position2D,
            0f,
            assetName,
            0f);
    }

    public static void DrawPlayer(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        _player.Draw(spriteBatch);
        spriteBatch.End();
    }


    public static void MovePlayerForward()
    {
        _player.Move(
            -Vector2.UnitY,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public static void MovePlayerBackward()
    {
        _player.Move(
            Vector2.UnitY,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public static void MovePlayerRight()
    {
        _player.Move(
            Vector2.UnitX,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public static void MovePlayerLeft()
    {
        _player.Move(
            -Vector2.UnitX,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
}