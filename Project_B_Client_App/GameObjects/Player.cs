using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Interface;

namespace Project_B_Client_App.GameObjects;

// The player object
public class Player(
    Texture2D texture2D, Vector2 position2D, float rotation, string assetName, float layerDepth,
    float moveSpeed = 100f) :
    GameObject(texture2D, position2D, rotation, assetName, layerDepth),
    IDawableObject
{
    public float GetSpeed => moveSpeed;
    
    public void Move(Vector2 direction, float deltaTime)
    {
        position2D += direction * (moveSpeed * deltaTime);
        rotation = TranslateDirectionToRotation(direction);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture2D,
            position2D,
            null,
            Color.White,
            rotation,
            new Vector2(texture2D.Width / 2, texture2D.Height / 2),
            Vector2.One / 3f,
            SpriteEffects.None,
            layerDepth);
    }
    
    private float TranslateDirectionToRotation(Vector2 direction)
    {
        return (float)Math.Atan2(direction.Y, direction.X);
    }
}