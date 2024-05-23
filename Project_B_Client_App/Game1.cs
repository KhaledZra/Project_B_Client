using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;
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
        private OrthographicCamera _camera;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;
        private Map _map;
        
        // todo: debug feature
        private bool _mouseReleased = true;
        private List<string> _tileCode = new List<string>();
        
        // Helps draw lines
        private Texture2D _pixel;

        //private TestObject _testObject;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            // Client size setup
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
            
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
                new Vector2(425, 875),
                "Sprites/player_sprite");

            //_testObject = new TestObject(PlayerController.GetPlayerPosition(), Content);
            
            // Register Handlers
            ServerHubHandler.SyncAlreadyConnectedPlayersHandler(this.Content);
            ServerHubHandler.AddNewConnectedOtherPlayerHandler(this.Content);
            ServerHubHandler.UpdateOtherPlayersHandler();
            ServerHubHandler.RemoveDisconnectedOtherPlayerHandler();
            
            base.Initialize();
            
            // Camera setup
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 400, 240);
            _camera = new OrthographicCamera(viewportadapter);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _tiledMap = Content.Load<TiledMap>("Map/samplemap_2");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            
            _map = new Map(_tiledMap.Width, _tiledMap.Height, new Point(_tiledMap.TileWidth, _tiledMap.TileHeight));
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData([Color.White]); // Fill the texture with white color
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
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
            
            // // Camera logic
            // TODO: Camera logic for clamping is not working as intended. Need to fix this.
            // var cameraPosition = PlayerController.GetPlayerPosition() - new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            // var minCameraPosition = Vector2.Zero;
            // var maxCameraPosition = new Vector2(_tiledMap.WidthInPixels - _graphics.PreferredBackBufferWidth, _tiledMap.HeightInPixels - _graphics.PreferredBackBufferHeight);
            //
            // // Clamp the camera position to the map bounds
            // cameraPosition = Vector2.Clamp(cameraPosition, minCameraPosition, maxCameraPosition);
            
            _camera.LookAt(PlayerController.GetPlayerPosition());
            
            // Animation logic
            InputController.Update();
            PlayerController.Update(gameTime, _tiledMap.WidthInPixels, _tiledMap.HeightInPixels, _map.CanMoveTo);
            
            // Todo: remove this after
            // Log.Information("{0}", _map.GetTileFromPosition(PlayerController.GetPlayerPosition()).GetBound());
            
            
            // If connected to the server, checks the player info sent to the server to see if they are done.
            // This also only runs if the player is connected to the server.
            GameController.CheckServerPlayerInfoCalls();
            GameController.OtherPlayers.ForEach(op => op.Update(gameTime));

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
            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());
            GameObjectController.DrawGameObjects(_spriteBatch);
            PlayerController.DrawPlayer(_spriteBatch);
            GameController.OtherPlayers.ForEach(op => op.Draw(_spriteBatch));
            // TODO: this is a debug feature. Turn it off later :)
            _map.Draw(_spriteBatch, _pixel);
            //_testObject.Draw(_spriteBatch);
            _spriteBatch.End();
            
            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Log.Information("Exiting game...");
            // todo remove later
            // File.WriteAllLines("tiles.txt", _tileCode.ToArray());
            // Log.Information("Saving tiles to textfile");
            base.OnExiting(sender, args);
        }


        // Debug feature
        private void SetTilesWithMouse()
        {
            var mouse = Mouse.GetState();
            Vector2 mousePosition = mouse.Position.ToVector2();
            Vector2 worldPosition = _camera.ScreenToWorld(mousePosition);
            
            if (mouse.LeftButton == ButtonState.Pressed && _mouseReleased)
            {
                _mouseReleased = false;
                var tilePoint = _map.GetTileFromPosition(worldPosition).GetTilePosition();
                if (_map.GetTileFromPosition(worldPosition).GetTileType() == TileType.Blocked)
                {
                    _map.GetTileFromPosition(worldPosition).SetTileType(TileType.Walkable);
                    _tileCode.Add($"_tileMap[{tilePoint.X}, {tilePoint.Y}].SetTileType(TileType.Walkable);");
                    Log.Information("Tile set to walkable at {0}", tilePoint);
                }
                else
                {
                    _map.GetTileFromPosition(worldPosition).SetTileType(TileType.Blocked);
                    _tileCode.Remove($"_tileMap[{tilePoint.X}, {tilePoint.Y}].SetTileType(TileType.Walkable);");
                }
            }
            else if (_mouseReleased == false && mouse.LeftButton == ButtonState.Released)
            {
                _mouseReleased = true;
                
            }
        }
    }
}
