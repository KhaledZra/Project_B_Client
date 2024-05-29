using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using Project_B_Client_App.Controllers;
using Project_B_Client_App.Enums;
using Project_B_Client_App.GameObjects;
using Project_B_Client_App.Handlers;
using Serilog;

namespace Project_B_Client_App
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly Vector2 _middleOfScreen;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private Map _map;
        private Camera _camera;
        private SpriteFont _spriteFont;

        // todo: debug feature
        private DebugTools _debugTools;

        // Helps draw lines
        private Texture2D _pixel;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            // Client size setup
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _middleOfScreen = new Vector2(_graphics.PreferredBackBufferWidth / 2.0f,
                _graphics.PreferredBackBufferHeight / 2.0f);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create player that spawns in a set spot on the map
            PlayerController.InitializePlayer(
                this.Content,
                new Vector2(425, 875));

            // Register Handlers
            ServerHubHandler.SyncAlreadyConnectedPlayersHandler(this.Content);
            ServerHubHandler.AddNewConnectedOtherPlayerHandler(this.Content);
            ServerHubHandler.UpdateOtherPlayersHandler();
            ServerHubHandler.RemoveDisconnectedOtherPlayerHandler();

            base.Initialize();

            // Camera setup
            _camera = new Camera(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

            if (Globals.IsDebugging)
            {
                _debugTools = new DebugTools();
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _tiledMap = Content.Load<TiledMap>("Map/samplemap_2");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

            _map = new Map(_tiledMap.Width, _tiledMap.Height, new Point(_tiledMap.TileWidth, _tiledMap.TileHeight));
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData([Color.White]); // Fill the texture with white color

            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _spriteFont = Content.Load<SpriteFont>("Font/8bit");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            this.Content.Unload();

            _spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) Exit();

            Globals.UpdateGt(gameTime);
            // TODO: Maybe group up the connection stuff to a separate method and call it here to reduce clutter and make a call order.
            // Will run one time and connect to the server, needs to be checked until it is connected.
            GameController.ConnectToServer();
            _tiledMapRenderer.Update(gameTime);

            // Camera logic
            _camera.CenterOn(PlayerController.GetPlayerPosition());

            // Animation logic
            InputController.Update();
            PlayerController.Update(gameTime, _tiledMap.WidthInPixels, _tiledMap.HeightInPixels, _map.CanMoveTo);


            // If connected to the server, checks the player info sent to the server to see if they are done.
            // This also only runs if the player is connected to the server.
            GameController.CheckServerPlayerInfoCalls();
            GameController.OtherPlayers.ForEach(op => op.Update(gameTime));

            if (Globals.IsDebugging)
            {
                _debugTools.SetTilesWithMouse(_camera, _map);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Background color
            GraphicsDevice.Clear(Color.Black);

            // Draw all game objects
            _spriteBatch.Begin(transformMatrix: _camera.TranslationMatrix);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            _tiledMapRenderer.Draw(_camera.TranslationMatrix);
            GameController.OtherPlayers.ForEach(op => op.Draw(_spriteBatch, _spriteFont));
            PlayerController.DrawPlayer(_spriteBatch, _spriteFont);
            
            // TODO: this is a debug feature. Turn it off later :)
            if (Globals.IsDebugging)
            {
                _map.Draw(_spriteBatch, _pixel);
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Log.Information("Exiting game...");
            if (Globals.IsDebugging)
            {
                _debugTools.SaveTileCode();
            }
            base.OnExiting(sender, args);
        }
    }
}