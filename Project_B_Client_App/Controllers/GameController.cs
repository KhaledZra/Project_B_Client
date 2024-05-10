using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_B_Client_App.GameObjects;
using Project_B_Client_App.Services;

namespace Project_B_Client_App.Controllers;

// The controller for the game, mostly to reduce clutter in the main class and to keep the game logic separate
public static class GameController
{
    public static GameTime GameTime { get; private set; }

    private static ServerHubConnectionService _serverHubConnectionService = new();
    private static Task _serverCall = _serverHubConnectionService.StartConnection();
    private static bool _isConnected = false;
    private static List<Task> _serverPlayerInfoCalls = new();
    private static Task<List<string>> _serverClientSync = _serverHubConnectionService.SendClientsInfoToCaller();
    
    // Todo: maybe move this to it's own class
    public static List<Player> OtherPlayers { get; set; } = new();

    // TODO: in the future in the server side, the server will send the spawn point
    // TODO: This is not working atm
    public static void SyncAlreadyConnectedPlayers(ContentManager content, Vector2 spawnPoint)
    {
        if (_serverClientSync is not null && _isConnected)
        {
            if (_serverClientSync.IsCompleted)
            {
                var clients = _serverClientSync.Result;
                // Clear this as we are going to refill it with the current active players
                OtherPlayers.Clear();
                Console.WriteLine("clients.Count: " + clients.Count);
                
                clients.ForEach(client =>
                {
                    // Incase the client is the current player
                    if (!client.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase))
                    {
                        Player player = new Player(content.Load<Texture2D>("Sprites/player_sprite"),
                            spawnPoint, 0f, "Sprites/player_sprite", 0f, client);
                        OtherPlayers.Add(player);
                    }
                });

                // TODO: Clear the task, might have to figure out a better way if i wanna reuse this task
                _serverClientSync = null;
            }
        }
    }

    public static void AddNewConnectedOtherPlayer(ContentManager content, Vector2 spawnPoint)
    {
        _serverHubConnectionService.ListenToReceiveNewClientNotification(payload =>
        {
            if (!string.IsNullOrWhiteSpace(payload))
            {
                // Ignore if it's about the client player or the payload is already processed
                // TODO: Bug here, the payload seems to be sending multiple times. Not sure if it's normal?
                if (payload.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase)) return;
                if (OtherPlayers.Exists(p =>
                        payload.Equals(p.GetPlayerName, StringComparison.OrdinalIgnoreCase))) return;
                Console.WriteLine("New player connected: " + payload);
                
                // Create a new player
                Player player = new Player(content.Load<Texture2D>("Sprites/player_sprite"),
                    new Vector2(spawnPoint.X, spawnPoint.Y), 0f, "Sprites/player_sprite", 0f, payload);
                OtherPlayers.Add(player);
            }
        });
    }

    public static void UpdateOtherPlayers()
    {
        // When the server sends a position, update the player position
        _serverHubConnectionService.ListenToReceivePosition(payload =>
        {
            if (payload is not null)
            {
                // Ignore if it's about the client player
                if (!payload.User.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase))
                {
                    // Find the player
                    Player player = OtherPlayers.Find(p =>
                        p.GetPlayerName.Equals(payload.User, StringComparison.OrdinalIgnoreCase));
                    if (player is not null)
                    {
                        player.SetPosition2D(new Vector2(payload.X, payload.Y));
                        player.SetRotation(payload.RotationRadians);
                    }
                }
            }
        });
    }


    public static void Update(GameTime gameTime)
    {
        GameTime = gameTime;
    }

    public static void InitializeGameInputs(Action exitGame)
    {
        // Initialize the game inputs
        // Movement
        InputController.AddInputAction(Keys.W, [PlayerController.MovePlayerForward, SendPlayerInfoToServer]);
        InputController.AddInputAction(Keys.S, [PlayerController.MovePlayerBackward, SendPlayerInfoToServer]);
        InputController.AddInputAction(Keys.D, [PlayerController.MovePlayerRight, SendPlayerInfoToServer]);
        InputController.AddInputAction(Keys.A, [PlayerController.MovePlayerLeft, SendPlayerInfoToServer]);

        // Exit
        InputController.AddInputAction(Keys.Escape, [exitGame]);
}
    
    public static void ConnectToServer()
    {
        if (_serverCall is not null && _serverHubConnectionService.GetState() != "Connecting")
        {
            // Check if server call is done
            if (_serverCall.IsCompleted)
            {
                if (_serverHubConnectionService.GetState() == "Connected")
                    _isConnected = true;
                
                _serverCall = null;
                Console.WriteLine("Server call is done!");
                Console.WriteLine(_serverHubConnectionService.GetState());
            }
        }
    }
    
    public static void SendPlayerInfoToServer()
    {
        if (_isConnected)
        {
            _serverPlayerInfoCalls.Add(_serverHubConnectionService.SendPlayerInfo(
                PlayerController.GetPlayerName(),
                PlayerController.GetPlayerPosition(),
                PlayerController.GetPlayerRotation()));
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