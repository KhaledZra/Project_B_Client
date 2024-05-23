using System;
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
            0f,
            new Random(Guid.NewGuid().GetHashCode()).Next().ToString(),  // TODO: Change later to get the player name from the UI
            contentManager);
    }

    public static string GetPlayerName() => _player.GetPlayerName;
    public static string GetPlayerSpriteName() => _player.GetPlayerSpriteName;
    public static Vector2 GetPlayerPosition() => _player.GetPlayerPosition;
    public static float GetPlayerRotation() => _player.GetRotation;

    public static void DrawPlayer(SpriteBatch spriteBatch)
    {
        _player.Draw(spriteBatch);
    }

    public static void Update(GameTime gameTime, int mapWidth, int mapHeight, Predicate<Vector2> canMoveAction)
    { 
        _player.Update(gameTime, mapWidth, mapHeight, canMoveAction);
    }
}