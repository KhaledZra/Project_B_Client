using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Handlers;

namespace Project_B_Client_App
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly Vector2 _middleOfScreen;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _middleOfScreen = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            // Create player that spawns in the middle of the game window
            // TODO: Clean up later how player is created
            PlayerController.InitializePlayer(
                this.Content, 
                _middleOfScreen, 
                "Sprites/player_sprite");
            
            GameController.InitializeGameInputs(Exit);
            
            // Register Handlers
            ServerHubHandler.AddNewConnectedOtherPlayerHandler(this.Content, _middleOfScreen);
            ServerHubHandler.UpdateOtherPlayersHandler();
            ServerHubHandler.SyncAlreadyConnectedPlayersHandler(this.Content, _middleOfScreen);
            ServerHubHandler.RemoveDisconnectedOtherPlayerHandler();
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            // TODO: This might be redundant. Keep an eye on this
            GameObjectController.LoadGameObjectsTextures(this.Content);
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            this.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Maybe group up the connection stuff to a separate method and call it here to reduce clutter and make a call order.
            // Will run one time and connect to the server, needs to be checked until it is connected.
            GameController.ConnectToServer();
            
            // If connected to the server, checks the player info sent to the server to see if they are done.
            // This also only runs if the player is connected to the server.
            GameController.CheckServerPlayerInfoCalls();
            
            GameController.Update(gameTime);
            
            InputController.OnInputAction(Keyboard.GetState().GetPressedKeys());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Background color
            GraphicsDevice.Clear(Color.CornflowerBlue);
            
            // Draw all game objects
            _spriteBatch.Begin();
            GameObjectController.DrawGameObjects(_spriteBatch);
            PlayerController.DrawPlayer(_spriteBatch);
            GameController.OtherPlayers.ForEach(player => player.Draw(_spriteBatch));
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }
    }
}
