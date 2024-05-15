using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Controllers;

namespace Project_B_Client_App.Controllers;

public class TestObject
{
    private static Texture2D _texture;
    private Vector2 _position;
    private readonly AnimationController _anims;

    public TestObject(Vector2 pos, ContentManager contentManager)
    {
        _anims = new AnimationController();
        _texture ??= contentManager.Load<Texture2D>("Animation/player1_spritesheet");
        _anims.AddAnimation(Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 1)); // Down
        _anims.AddAnimation(-Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 2)); // Left
        _anims.AddAnimation(Vector2.UnitX, new Animation(_texture, 4, 4, 0.1f, 3)); // Right
        _anims.AddAnimation(-Vector2.UnitY, new Animation(_texture, 4, 4, 0.1f, 4));// Up
        _position = pos;
    }

    public void Update(GameTime gameTime)
    {
        if (InputController.Moving)
        {
            _position += Vector2.Normalize(InputController.Direction) * 70.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        _anims.Update(InputController.Direction, gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _anims.Draw(_position, spriteBatch);
    }
}