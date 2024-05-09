using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.GameObjects;

namespace Project_B_Client_App.Content.Controllers;

public static class PlayerController
{
    public static Player CreatePlayer(ContentManager contentManager, Vector2 position2D, String assetName)
    {
        return new Player(
            contentManager.Load<Texture2D>(assetName),
            position2D,
            0f,
            assetName,
            0f);
    }
    
    public static void MovePlayerForward(Player player)
    {
        // TODO: Move player forward
        throw new NotImplementedException();
    }
}