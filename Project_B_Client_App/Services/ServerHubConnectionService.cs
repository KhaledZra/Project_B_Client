using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Payloads;

namespace Project_B_Client_App.Services;

public class ServerHubConnectionService
{
    private readonly HubConnection _hubConnection;
    // private const string Url = "https://project-b-server-081b429cac7e.herokuapp.com/chathub";
    private const string Url = "http://localhost:5013/chathub";
    
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
        await _hubConnection.InvokeAsync("SendClientInfo", PlayerController.GetPlayerName());
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
    
    public async Task SendPlayerInfo(string user, Vector2 playerPosition, float rotationRadians)
    {
        await _hubConnection.InvokeAsync("SendPosition", user, playerPosition.X, playerPosition.Y, rotationRadians);
    }

    public void ListenToGetOtherConnectedClients(Action<List<string>> handler)
    {
        _hubConnection.On("ReceiveClientsInfo", handler);
    }

    public void ListenToReceivePosition(Action<ReceivePositionPayload> handler)
    {
        _hubConnection.On("ReceivePosition", (string user, float x, float y, float rotationRadians) =>
            handler(new ReceivePositionPayload(user, x, y, rotationRadians)));
    }
    
    public void ListenToReceiveNewClientNotification(Action<string> handler)
    {
        _hubConnection.On("ReceiveNewClientNotification", handler);
    }
    
    public void ListenToReceiveClientDisconnectedNotification(Action<string> handler)
    {
        _hubConnection.On("ReceiveClientDisconnectedNotification", handler);
    }
}