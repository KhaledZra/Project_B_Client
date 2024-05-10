using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.GameObjects;
using Project_B_Client_App.Interface;

namespace Project_B_Client_App.Controllers;

// Manages all GameObjects
public static class GameObjectController
{
    private static List<GameObject> _gameObjects = new();
    
    public static void AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }
    
    public static void DrawGameObjects(SpriteBatch spriteBatch)
    {
        foreach (var gameObject in _gameObjects)
        {
            if (gameObject is IDawableObject drawableObject)
            {
                drawableObject.Draw(spriteBatch);
            }
        }
    }
    
    public static void LoadGameObjectsTextures(ContentManager content)
    {
        foreach (var gameObject in _gameObjects)
        {
            gameObject.SetTexture2D(content.Load<Texture2D>(gameObject.GetAssetName()));
        }
    }
}