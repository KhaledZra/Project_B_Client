using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Interface;

namespace Project_B_Client_App.Controllers;

public class Floor(Texture2D texture2D, Vector2 position2D, float rotation, string assetName, float layerDepth) :
    GameObject(texture2D, position2D, rotation, assetName, layerDepth), IDrawableObject
{
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture2D,
            position2D,
            new Rectangle(0, 0, 80, 80),
            Color.White,
            rotation,
            new Vector2(texture2D.Width / 2, texture2D.Height / 2),
            Vector2.One,
            SpriteEffects.None,
            layerDepth);
    }
}