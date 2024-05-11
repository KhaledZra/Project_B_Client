using Serilog;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Logger = log;

Log.Logger.Information("Starting Project B Client App");

using var game = new Project_B_Client_App.Game1();
game.Run();