using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project_B_Client_App.Controllers;

// The base class of all GameObjects
public abstract class GameObject(
    Texture2D texture2D,
    Vector2 position2D,
    float rotation,
    string assetName,
    float layerDepth)
{
    public void SetTexture2D(Texture2D texture) => texture2D = texture;
    public string GetAssetName() => assetName;
}