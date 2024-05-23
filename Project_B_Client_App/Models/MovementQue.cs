using Microsoft.Xna.Framework;

namespace Project_B_Client_App.Models;

public class MovementQue (Vector2 direction, Vector2 position)
{
    public Vector2 GetDirection => direction;
    public Vector2 GetPosition => position;
}