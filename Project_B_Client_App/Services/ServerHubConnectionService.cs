using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Payloads;

namespace Project_B_Client_App.Services;

public class ServerHubConnectionService
{
    private readonly HubConnection _hubConnection;
    // private const string Url = "https://project-b-server-081b429cac7e.herokuapp.com/serverhub";
    private const string Url = "http://localhost:5013/serverhub";
    
    public ServerHubConnectionService()
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl(Url)
            .Build();
    }
    
    public async Task StartConnection()
    {
        await _hubConnection.StartAsync();
        // Tell server this player has connected
        var playerPos = PlayerController.GetPlayerPosition();
        await _hubConnection.InvokeAsync("SendClientInfo", PlayerController.GetPlayerName(), playerPos.X, playerPos.Y);
        await _hubConnection.InvokeAsync("SendClientsInfoToCaller");
    }

    public async Task StopConnection()
    {
        await _hubConnection.StopAsync();
    }

    public string GetState()
    {
        return _hubConnection.State.ToString();
    }
    
    public async Task SendMessage(string user, string message)
    {
        await _hubConnection.InvokeAsync("SendMessage", user, message);
    }
    
    public async Task SendPlayerInfo(string user, Vector2 playerPosition, float rotationRadians, Vector2 direction)
    {
        await _hubConnection.InvokeAsync("SendPosition",
            user, playerPosition.X, playerPosition.Y, rotationRadians, direction.X, direction.Y);
    }

    
    public void ListenToGetOtherConnectedClients(Action<string> handler)
    {
        _hubConnection.On("ReceiveClientsInfo", handler);
    }

    public void ListenToReceivePosition(Action<ReceivePositionPayload> handler)
    {
        _hubConnection.On("ReceivePosition", (string user, float x, float y, float rotationRadians, float directionX, float directionY) =>
            handler(new ReceivePositionPayload(user, x, y, rotationRadians, new Vector2(directionX, directionY))));
    }
    
    public void ListenToReceiveNewClientNotification(Action<ClientPayload> handler)
    {
        _hubConnection.On("ReceiveNewClientNotification", (string user, float positionX, float positionY) =>
            handler(new ClientPayload(user, positionX, positionY)));
    }
    
    public void ListenToReceiveClientDisconnectedNotification(Action<string> handler)
    {
        _hubConnection.On("ReceiveClientDisconnectedNotification", handler);
    }
}