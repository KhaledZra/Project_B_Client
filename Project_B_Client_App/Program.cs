using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using Project_B_Client_App;
using Project_B_Client_App.Models;
using Serilog;


using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger = log;

// Forcing user to go through launcher
if(args.Length == 0)
{
    Log.Error("Please use the launcher to start the game");
    try
    {
        Process.Start("Launcher/Project_B_Launcher.exe");
        return;
    }
    catch (Exception e)
    {
        // If there is an error when starting the launcher, launch game anyway and log the error
        Log.Error(e, "Error starting launcher");
    }
}

// Loading config
try
{
    string jsonString = File.ReadAllText("Launcher/config.json");
    Globals.Config = JsonSerializer.Deserialize<Config>(jsonString);
    Log.Information("Config loaded successfully");
}
catch (Exception e)
{
    Log.Error(e, "Failed to load config, continuing with default config");
}
Log.Information("Player nickname: {ConfigPlayerName} - Player Asset: {ConfigPlayerAsset}",
    Globals.Config.PlayerName, Globals.Config.PlayerAsset);

// Starting the game
Log.Information("Starting Project B Client App");
using var game = new Project_B_Client_App.Game1();
game.Run();