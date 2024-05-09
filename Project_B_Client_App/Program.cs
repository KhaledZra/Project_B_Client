using Microsoft.AspNetCore.SignalR.Client;

// var hubConnection = new HubConnectionBuilder()
//     // Change this to property
//     //.WithUrl("http://localhost:5013/chathub")
//     .WithUrl("https://project-b-server-081b429cac7e.herokuapp.com/chathub")
//     .Build();
// // the on statement goes here
// // Example:
// // hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
// // {
// //     // TODO the time will not be 1=1 since this is set locally vs the one saved in the database
// //     messages.Add(new Message { User = user, Text = message, DateStamp = DateTime.Now });
// //     InvokeAsync(StateHasChanged);
// // });
//
// await hubConnection.StartAsync();

using var game = new Project_B_Client_App.Game1();
game.Run();
