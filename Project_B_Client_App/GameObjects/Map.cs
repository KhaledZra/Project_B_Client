using Microsoft.Xna.Framework;
using Project_B_Client_App.Enums;
using Serilog;

namespace Project_B_Client_App.GameObjects;

public class Map
{
    private int _width;
    private int _height;
    private Point _tileSize;
    private Tile[,] _tileMap;
    
    public Map(int width, int height, Point tileSize)
    {
        _width = width;
        _height = height;
        _tileSize = tileSize;
        _tileMap = new Tile[_width, _height];
        CreateMap();
    }

    private void CreateMap()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Rectangle tile = new Rectangle(x * _tileSize.X, y * _tileSize.Y, _tileSize.X, _tileSize.Y);
                _tileMap[x, y] = new Tile(tile, TileType.Walkable);
            }
        }
    }
    
    public bool CanMoveTo(Vector2 position)
    {
        if (position.X < 0 || position.X >= _width || position.Y < 0 || position.Y >= _height)
        {
            return false;
        }
        
        
        return GetTileFromPosition(position).CanMoveTo((int)position.X, (int)position.Y);
    }
    
    public Tile GetTileFromPosition(Vector2 position)
    {
        int x = (int)position.X / _tileSize.X;
        int y = (int)position.Y / _tileSize.Y;
        
        return _tileMap[x, y];
    }
}