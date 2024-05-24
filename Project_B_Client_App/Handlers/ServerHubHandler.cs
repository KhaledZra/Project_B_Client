using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.GameObjects;
using Project_B_Client_App.Models;
using Project_B_Client_App.Payloads;
using Serilog;

namespace Project_B_Client_App.Handlers;

public static class ServerHubHandler
{
    public static void SyncAlreadyConnectedPlayersHandler(ContentManager content)
    {
        GameController.ServerHubConnectionService.ListenToGetOtherConnectedClients(
            payload =>
            {
                Log.Information("Received clients info: {Payload}", payload);
                List<ClientPayload> clientPayloads = JsonSerializer.Deserialize<List<ClientPayload>>(payload);
                // Clear this as we are going to refill it with the current active players
                GameController.OtherPlayers.Clear();
                Log.Information("clients.Count: {Count}", clientPayloads.Count);

                clientPayloads.ForEach(client =>
                {
                    // Incase the client is the current player
                    if (!client.ClientName.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase))
                    {
                        OtherPlayer otherPlayer = new OtherPlayer(
                            content.Load<Texture2D>(client.ClientPlayerSprite),
                            new Vector2(client.PositionX, client.PositionY),
                            0f,
                            client.ClientPlayerSprite,
                            0f,
                            client.ClientName,
                            client.ClientNickName);

                        GameController.OtherPlayers.Add(otherPlayer);
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
                    OtherPlayer player = GameController.OtherPlayers.Find(p =>
                        p.GetPlayerName.Equals(payload.User, StringComparison.OrdinalIgnoreCase));
                    if (player is not null)
                    {
                        Console.WriteLine("Received position: " + payload.User + " " + payload.Direction);
                        player.AddDirectionStack(new MovementQue(payload.Direction, new Vector2(payload.X, payload.Y)));
                    }
                }
            }
        });
    }

    public static void AddNewConnectedOtherPlayerHandler(ContentManager content)
    {
        GameController.ServerHubConnectionService.ListenToReceiveNewClientNotification(payload =>
        {
            // Ignore if it's about the client player or the payload is already processed
            if (payload.ClientName.Equals(PlayerController.GetPlayerName(), StringComparison.OrdinalIgnoreCase)) return;
            Log.Information("New player connected: {Payload}", payload);

            // Create a new player
            OtherPlayer player = new OtherPlayer(
                content.Load<Texture2D>(payload.ClientPlayerSprite),
                new Vector2(payload.PositionX, payload.PositionY),
                0f,
                payload.ClientPlayerSprite,
                0f,
                payload.ClientName,
                payload.ClientNickName);

            GameController.OtherPlayers.Add(player);
            Log.Information("Player added to the list! {OtherPlayersCount}", GameController.OtherPlayers.Count);
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
                Log.Information("Player disconnected: {Payload}", payload);

                // Find the player
                OtherPlayer player = GameController.OtherPlayers.Find(p =>
                    p.GetPlayerName.Equals(payload, StringComparison.OrdinalIgnoreCase));
                if (player is not null)
                {
                    GameController.OtherPlayers.Remove(player);
                }
            }
        });
    }
}