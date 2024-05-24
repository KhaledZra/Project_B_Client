using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.GameObjects;

namespace Project_B_Client_App.Controllers;

public static class PlayerController
{
    private static Player _player;
    
    public static void InitializePlayer(ContentManager contentManager, Vector2 position2D)
    {
        _player = new(
            contentManager.Load<Texture2D>(Globals.Config.PlayerAsset),
            position2D,
            0f,
            Globals.Config.PlayerAsset,
            0f,
            new Random(Guid.NewGuid().GetHashCode()).Next().ToString(),  // TODO: Change later to get the player name from the UI
            contentManager,
            Globals.Config.PlayerName);
    }

    public static string GetPlayerName() => _player.GetPlayerName;
    public static string GetPlayerNickName() => _player.GetPlayerNickName;
    public static string GetPlayerSpriteName() => _player.GetPlayerSpriteName;
    public static Vector2 GetPlayerPosition() => _player.GetPlayerPosition;
    public static float GetPlayerRotation() => _player.GetRotation;

    public static void DrawPlayer(SpriteBatch spriteBatch, SpriteFont font)
    {
        _player.Draw(spriteBatch);
        
        // Calculate font position
        var fontPos = _player.GetPlayerPosition;
        // todo fix this logic
        fontPos.Y -= font.MeasureString(_player.GetPlayerNickName).Y / 2.0f;
        fontPos.X -= 13;
        spriteBatch.DrawString(font, _player.GetPlayerNickName, fontPos, Color.White, 0f, Vector2.Zero, Vector2.One / 4.0f, SpriteEffects.None, 0f);
    }

    public static void Update(GameTime gameTime, int mapWidth, int mapHeight, Predicate<Vector2> canMoveAction)
    { 
        _player.Update(gameTime, mapWidth, mapHeight, canMoveAction);
    }
}