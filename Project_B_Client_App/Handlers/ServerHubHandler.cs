using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Controllers;
using Serilog;

namespace Project_B_Client_App.Handlers;

public class ServerHubHandler
{
    // TODO: in the future in the server side, the server will send the spawn point
    // TODO: This is not working atm
    public static void SyncAlreadyConnectedPlayersHandler(ContentManager content, Vector2 spawnPoint)
    {
        // // Player is synced with the server regarding the other players
        // if (!_isOtherPlayerSyncNeeded) return;
        //
        // // Set to false as the handler is now registered
        // _isOtherPlayerSyncNeeded = false;
        
        GameController.ServerHubConnectionService.ListenToGetOtherConnectedClients(
            payload =>
            {
                // Clear this as we are going to refill it with the current active players
                GameController.OtherPlayers.Clear();
                Log.Information("clients.Count: " + payload.Count);

                payload.ForEach(client =>
                {
                    // Incase the client is the current player
                    if (!client.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase))
                    {
                        Player player = new Player(
                            content.Load<Texture2D>("Sprites/player_sprite"),
                            spawnPoint,
                            0f,
                            "Sprites/player_sprite",
                            0f,
                            client,
                            content);

                        GameController.OtherPlayers.Add(player);
                    }
                });
            });
    }
    
    public static void UpdateOtherPlayersHandler()
    {
        // When the server sends a position, update the player position
        GameController.ServerHubConnectionService.ListenToReceivePosition(payload =>
        {
            if (payload is not null)
            {
                // Ignore if it's about the client player
                if (!payload.User.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase))
                {
                    // Find the player
                    Player player = GameController.OtherPlayers.Find(p =>
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
    
    public static void AddNewConnectedOtherPlayerHandler(ContentManager content, Vector2 spawnPoint)
    {
        GameController.ServerHubConnectionService.ListenToReceiveNewClientNotification(payload =>
        {
            if (!string.IsNullOrWhiteSpace(payload))
            {
                // Ignore if it's about the client player or the payload is already processed
                if (payload.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase)) return;
                // Todo: remove this if everything works like normal!
                // if (OtherPlayers.Exists(p =>
                //         payload.Equals(p.GetPlayerName, StringComparison.OrdinalIgnoreCase))) return;
                Log.Information("New player connected: " + payload);

                // Create a new player
                Player player = new Player(content.Load<Texture2D>("Sprites/player_sprite"),
                    new Vector2(spawnPoint.X, spawnPoint.Y), 0f, "Sprites/player_sprite",
                    0f, payload, content);
                GameController.OtherPlayers.Add(player);
            }
        });
    }
    
    public static void RemoveDisconnectedOtherPlayerHandler()
    {
        GameController.ServerHubConnectionService.ListenToReceiveClientDisconnectedNotification(payload =>
        {
            if (!string.IsNullOrWhiteSpace(payload))
            {
                // Ignore if it's about the client player, should not really happen but just in case.
                if (payload.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase)) return;
                Log.Information("Player disconnected: " + payload);

                // Find the player
                Player player = GameController.OtherPlayers.Find(p =>
                    p.GetPlayerName.Equals(payload, StringComparison.OrdinalIgnoreCase));
                if (player is not null)
                {
                    GameController.OtherPlayers.Remove(player);
                }
            }
        });
    }
}