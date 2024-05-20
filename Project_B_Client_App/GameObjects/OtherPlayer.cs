using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Interface;
using Serilog;

namespace Project_B_Client_App.GameObjects;

public class OtherPlayer : GameObject, IDrawableObject
{
    private Texture2D _texture;
    private Vector2 _position;
    private float _rotation;
    private readonly float _moveSpeed;
    private readonly string _playerName;
    private readonly float _layerDepth;
    //private readonly AnimationController _anims;
    
    public float GetSpeed => _moveSpeed;
    public string GetPlayerName => _playerName;
    public Vector2 GetPlayerPosition => _position;
    public float GetRotation => _rotation;
    
    // create constructor
    public OtherPlayer(
        Texture2D texture2D, Vector2 position2D, float rotation, string assetName, float layerDepth, string playerName, ContentManager contentManager,
        float moveSpeed = 75f) :
        base(texture2D, position2D, rotation, assetName, layerDepth)
    {
        _texture = texture2D;
        _position = position2D;
        _rotation = rotation;
        _playerName = playerName;
        _layerDepth = layerDepth;
        _moveSpeed = moveSpeed;
        
        // Setup player animations
        // _anims = new AnimationController();
        // _anims.AddAnimation(Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 1)); // Down
        // _anims.AddAnimation(-Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 2)); // Left
        // _anims.AddAnimation(Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 3)); // Right
        // _anims.AddAnimation(-Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 4));// Up
    }
    
    public void SetPosition2D(Vector2 position) => _position = position;
    public void SetRotation(float rotationRadians) => _rotation = rotationRadians;
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            _texture,
            _position,
            null,
            Color.White,
            _rotation,
            new Vector2(_texture.Width / 2, _texture.Height / 2),
            Vector2.One / 2f,
            0f,
            0);
    }
}