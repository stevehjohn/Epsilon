using System.Collections.Generic;
using Epsilon.Actors;
using Epsilon.Controls;
using Epsilon.Coordination;
using Epsilon.Environment;
using Epsilon.Infrastructure.Configuration;
using Epsilon.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Infrastructure
{
    public class Epsilon : Game
    {
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private readonly EventManager _eventManager;
        private readonly Map _map;
        private readonly MouseTracker _mouseTracker;
        private readonly KeyboardTracker _keyBoardTracker;
        private readonly List<IActor> _actors;

        private SpriteBatch _spriteBatch;

        public Epsilon()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this)
                                     {
                                         PreferredBackBufferWidth = Constants.ScreenBufferWidth,
                                         PreferredBackBufferHeight = Constants.ScreenBufferHeight
                                     };

            Content.RootDirectory = "_Content";

            _eventManager = new EventManager();

            _map = new Map();

            _mouseTracker = new MouseTracker();
            _keyBoardTracker = new KeyboardTracker();

            // TODO: Maybe use some assembly scanning technique to pick all IActors up...
            _actors = new List<IActor>
                      {
                          new Stars(),
                          new Terrain(_map, _eventManager)
                      };
        }

        protected override void Initialize()
        {
            foreach (var actor in _actors)
            {
                actor.Initialise();
            }

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var actor in _actors)
            {
                actor.LoadContent(Content, _spriteBatch);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (IsActive)
            {
                _keyBoardTracker.TrackState();

                Keys? key;

                while ((key = _keyBoardTracker.GetKeyPress()) != null)
                {
                    switch (key)
                    {
                        case Keys.Right:
                            _map.Rotation = _map.Rotation == 270
                                                ? 0
                                                : _map.Rotation + 90;

                            _eventManager.RaiseEvent(EventType.RotationChanged);
                            break;
                        case Keys.Left:
                            _map.Rotation = _map.Rotation == 0
                                                ? 270
                                                : _map.Rotation - 90;

                            _eventManager.RaiseEvent(EventType.RotationChanged);
                            break;
                        case Keys.Up:
                            if (_keyBoardTracker.Ctrl)
                            {
                                GameState.WaterLevel++;
                            }

                            break;
                        case Keys.Down:
                            if (_keyBoardTracker.Ctrl)
                            {
                                GameState.WaterLevel--;
                            }

                            break;
                        case Keys.E:
                            AppSettings.Instance.Rendering.RenderBoardEdges = ! AppSettings.Instance.Rendering.RenderBoardEdges;
                            break;
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Up) && ! _keyBoardTracker.Ctrl)
                {
                    GameState.WaterLevel++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) && ! _keyBoardTracker.Ctrl)
                {
                    GameState.WaterLevel--;
                }

                var movement = _mouseTracker.GetMapMovement();

                _map.Move(movement);
            }

            foreach (var actor in _actors)
            {
                actor.UpdateState();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);

            var depth = 0.0f;

            foreach (var actor in _actors)
            {
                depth = actor.Render(depth);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _graphicsDeviceManager.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}