using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Enums;

namespace Project_B_Client_App.GameObjects;

public class Tile
{
    private Rectangle _bound;
    private TileType _type;

    public Tile(Rectangle bound, TileType type)
    {
        _bound = bound;
        _type = type;
    }

    public Rectangle GetBound()
    {
        return _bound;
    }

    public void SetTileType(TileType type)
    {
        _type = type;
    }

    public bool CanMoveTo(int x, int y)
    {
        if (_bound.Contains(x, y) && _type == TileType.Walkable) return true;

        return false;
    }

    public void Draw(SpriteBatch spriteBatch, Texture2D pixel)
    {
        Color tileColor = _type == TileType.Walkable ? Color.Blue : Color.Red;
        int borderWidth = 1; // Change this to change the width of the border

        // Top line
        spriteBatch.Draw(pixel, new Rectangle(_bound.X, _bound.Y, _bound.Width, borderWidth), tileColor);
        // Bottom line
        spriteBatch.Draw(pixel, new Rectangle(_bound.X, _bound.Y + _bound.Height - borderWidth, _bound.Width, borderWidth),
            tileColor);
        // Left line
        spriteBatch.Draw(pixel, new Rectangle(_bound.X, _bound.Y, borderWidth, _bound.Height), tileColor);
        // Right line
        spriteBatch.Draw(pixel, new Rectangle(_bound.X + _bound.Width - borderWidth, _bound.Y, borderWidth, _bound.Height),
            tileColor);
    }
}