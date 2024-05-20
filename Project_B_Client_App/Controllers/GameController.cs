using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Services;
using Serilog;

namespace Project_B_Client_App.Controllers;

// The controller for the game, mostly to reduce clutter in the main class and to keep the game logic separate
public static class GameController
{
    // Todo: refactor this later.
    public static readonly ServerHubConnectionService ServerHubConnectionService = new();
    private static Task _serverCall = ServerHubConnectionService.StartConnection();
    private static bool _isConnected = false;
    private static List<Task> _serverPlayerInfoCalls = new();

    // Todo: maybe move this to it's own class
    public static List<Player> OtherPlayers { get; set; } = new();

    public static void InitializeGameInputs(Action exitGame)
    {
        // Initialize the game inputs
        // Movement
        // InputController.AddInputAction(Keys.W, [PlayerController.MovePlayerForward, SendPlayerInfoToServer]);
        // InputController.AddInputAction(Keys.S, [PlayerController.MovePlayerBackward, SendPlayerInfoToServer]);
        // InputController.AddInputAction(Keys.D, [PlayerController.MovePlayerRight, SendPlayerInfoToServer]);
        // InputController.AddInputAction(Keys.A, [PlayerController.MovePlayerLeft, SendPlayerInfoToServer]);
        //
        // // Exit
        // InputController.AddInputAction(Keys.Escape, [exitGame]);
    }

    public static void ConnectToServer()
    {
        if (_serverCall is not null && ServerHubConnectionService.GetState() != "Connecting")
        {
            // Check if server call is done
            if (_serverCall.IsCompleted)
            {
                if (ServerHubConnectionService.GetState() == "Connected")
                    _isConnected = true;

                _serverCall = null;
                Log.Information("Server call is done!");
                Log.Information(ServerHubConnectionService.GetState());
            }
        }
    }

    // Updates the server with player info
    public static void SendPlayerInfoToServer(Vector2 direction)
    {
        if (_isConnected)
        {
            _serverPlayerInfoCalls.Add(ServerHubConnectionService.SendPlayerInfo(
                PlayerController.GetPlayerName(),
                PlayerController.GetPlayerPosition(),
                PlayerController.GetPlayerRotation(),
                direction));
        }
    }

    public static void CheckServerPlayerInfoCalls()
    {
        if (_serverPlayerInfoCalls.Count > 0 && _isConnected)
        {
            _serverPlayerInfoCalls.RemoveAll(call => call.IsCompleted);
        }
    }
}