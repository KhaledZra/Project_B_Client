using Microsoft.Xna.Framework;
using Project_B_Client_App.Models;

namespace Project_B_Client_App;

public static class Globals
{
    public static GameTime GameTime { get; set; }
    public static Config Config { get; set; } = new Config();
    
    public static void UpdateGt(GameTime gameTime) => GameTime = gameTime;
    
    public static float GetDeltaTimeFloat() => (float)GameTime.ElapsedGameTime.TotalSeconds;
}