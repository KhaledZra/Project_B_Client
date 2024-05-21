using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Interface;

namespace Project_B_Client_App.GameObjects;

// The player object
public class Player : GameObject, IDrawableObject
{
    private Texture2D _texture;
    private Vector2 _position;
    private float _rotation;
    private readonly float _moveSpeed;
    private readonly string _playerName;
    private readonly float _layerDepth;
    private readonly AnimationController _anims;
    
    public float GetSpeed => _moveSpeed;
    public string GetPlayerName => _playerName;
    public Vector2 GetPlayerPosition => _position;
    public float GetRotation => _rotation;

    
    // create constructor
    public Player(
        Texture2D texture2D, Vector2 position2D, float rotation, string assetName, float layerDepth, string playerName, ContentManager contentManager,
        float moveSpeed = 75f) :
        base(texture2D, position2D, rotation, assetName, layerDepth)
    {
        _texture = contentManager.Load<Texture2D>("Animation/player1_spritesheet");
        _position = position2D;
        _rotation = rotation;
        _playerName = playerName;
        _layerDepth = layerDepth;
        _moveSpeed = moveSpeed;
        
        // Setup player animations
        _anims = new AnimationController();
        _anims.AddAnimation(Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 1)); // Down
        _anims.AddAnimation(-Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 2)); // Left
        _anims.AddAnimation(Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 3)); // Right
        _anims.AddAnimation(-Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 4));// Up
    }
    
    public void Update(GameTime gameTime)
    {
        if (InputController.Moving)
        {
            _position += Vector2.Normalize(InputController.Direction) * 70.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
            GameController.SendPlayerInfoToServer(InputController.Direction);
            Console.WriteLine($"Player position: {_position}");
        }
        
        _anims.Update(InputController.Direction, gameTime);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        _anims.Draw(_position, spriteBatch);
    }
}