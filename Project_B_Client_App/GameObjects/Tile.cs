using Microsoft.Xna.Framework;
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
}