using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_B_Client_App.Enums;
using Project_B_Client_App.GameObjects;
using Serilog;

namespace Project_B_Client_App;

public class DebugTools
{
    private List<string> _tileCode = new List<string>();
    private bool _mouseReleased = false;
    
    public void SetTilesWithMouse(Camera camera, Map map)
    {
        var mouse = Mouse.GetState();
        Vector2 mousePosition = mouse.Position.ToVector2();
        Vector2 worldPosition = camera.ScreenToWorld(mousePosition);

        if (mouse.LeftButton == ButtonState.Pressed && _mouseReleased)
        {
            _mouseReleased = false;
            var tilePoint = map.GetTileFromPosition(worldPosition).GetTilePosition();
            if (map.GetTileFromPosition(worldPosition).GetTileType() == TileType.Blocked)
            {
                map.GetTileFromPosition(worldPosition).SetTileType(TileType.Walkable);
                _tileCode.Add($"_tileMap[{tilePoint.X}, {tilePoint.Y}].SetTileType(TileType.Walkable);");
                Log.Information("Tile set to walkable at {0}", tilePoint);
            }
            else
            {
                map.GetTileFromPosition(worldPosition).SetTileType(TileType.Blocked);
                _tileCode.Remove($"_tileMap[{tilePoint.X}, {tilePoint.Y}].SetTileType(TileType.Walkable);");
            }
        }
        else if (_mouseReleased == false && mouse.LeftButton == ButtonState.Released)
        {
            _mouseReleased = true;
        }
    }
    
    public void SaveTileCode()
    {
        File.WriteAllLines("tiles.txt", _tileCode.ToArray());
        Log.Information("Saving tiles to textfile");
    }
}