using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Content;
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Sprites;
using Project_B_Client_App.Controllers;

namespace Project_B_Client_App.Controllers;

public static class PlayerController
{
    private static Player _player;
    // private static AnimatedSprite _motwSprite;
    // private static string _animation; // Current animation
    
    public static void InitializePlayer(ContentManager contentManager, Vector2 position2D, String assetName)
    {
        // // Setup player animations
        // var spriteSheet = new JsonContentLoader().Load<SpriteSheet>(contentManager, "Animation/motw.xnb");
        // var sprite = new AnimatedSprite(spriteSheet);
        // _animation = "idle";
        // sprite.Play(_animation);
        // _motwSprite = sprite;
        
        // TODO change out texture loader to use the new sprite sheet
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
    public static Vector2 GetPlayerPosition() => _player.GetPlayerPosition;
    public static float GetPlayerRotation() => _player.GetRotation;

    public static void DrawPlayer(SpriteBatch spriteBatch)
    {
        // TODO REFACTOR
        _player.Draw(spriteBatch);
        
        //_player.AnimationDraw(spriteBatch, _motwSprite);
    }

    public static void Update(GameTime gameTime)
    { 
        _player.Update(gameTime);
    }

    // public static void PlayAnimations(GameTime gameTime)
    // {
    //     _motwSprite.Play(_animation);
    //     _motwSprite.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    //     _animation = "idle";
    // }
    
    public static void MovePlayerForward()
    {
        // _animation = "walkNorth";
        _player.Move(
            -Vector2.UnitY,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public static void MovePlayerBackward()
    {
        // _animation = "walkSouth";
        _player.Move(
            Vector2.UnitY,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public static void MovePlayerRight()
    {
        // _animation = "walkEast";
        _player.Move(
            Vector2.UnitX,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
    
    public static void MovePlayerLeft()
    {
        // _animation = "walkWest";
        _player.Move(
            -Vector2.UnitX,
            (float)GameController.GameTime.ElapsedGameTime.TotalSeconds);
    }
}