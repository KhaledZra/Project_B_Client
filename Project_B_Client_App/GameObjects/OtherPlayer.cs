using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Models;

namespace Project_B_Client_App.GameObjects;

public class OtherPlayer : GameObject
{
    private Texture2D _texture;
    private Vector2 _position;
    private float _rotation;
    private readonly float _moveSpeed;
    private readonly string _playerName;
    private readonly string _playerNickName;
    private readonly float _layerDepth;
    private readonly AnimationController _anims;
    private readonly string _assetName;
    
    // Movement
    private List<MovementQue> _directions;
    public void AddDirectionStack(MovementQue que) => _directions.Add(que);
    
    public float GetSpeed => _moveSpeed;
    public string GetPlayerName => _playerName;
    public string GetPlayerNickName => _playerNickName;
    public Vector2 GetPlayerPosition => _position;
    public float GetRotation => _rotation;
    
    // create constructor
    public OtherPlayer(
        Texture2D texture2D, Vector2 position2D, float rotation, string assetName, float layerDepth, string playerName,
        string playerNickName,
        float moveSpeed = 75f) :
        base(texture2D, position2D, rotation, assetName, layerDepth)
    {
        _texture = texture2D;
        _position = position2D;
        _rotation = rotation;
        _playerName = playerName;
        _layerDepth = layerDepth;
        _moveSpeed = moveSpeed;
        _assetName = assetName;
        _playerNickName = playerNickName;
        
        // Setup player animations
        _directions = new();
        _anims = new AnimationController();
        _anims.AddAnimation(Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 1)); // Down
        _anims.AddAnimation(-Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 2)); // Left
        _anims.AddAnimation(Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 3)); // Right
        _anims.AddAnimation(-Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 4));// Up
    }
    
    public void SetPosition2D(Vector2 position) => _position = position;
    public void SetRotation(float rotationRadians) => _rotation = rotationRadians;
    
    public void Update(GameTime gameTime)
    {
        if (_directions.Count > 0)
        {
            _position = _directions[0].GetPosition;
            _anims.Update(_directions[0].GetDirection, gameTime);
            _directions.RemoveAt(0);
        }
        else
        {
            _anims.Update(Vector2.Zero, gameTime);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
    {
        _anims.Draw(_position, spriteBatch);
        
        // Calculate font position
        var fontPos = _position;

        // Measure the size of the string
        Vector2 stringSize = spriteFont.MeasureString(_playerNickName);

        // Adjust the position to center the text above the player
        fontPos.X -= stringSize.X / 8f;
        fontPos.Y -= stringSize.Y / 2f;

        spriteBatch.DrawString(spriteFont, _playerNickName, fontPos, Color.White, 0f, Vector2.Zero, Vector2.One / 4.0f, SpriteEffects.None, 0f);
    }
}