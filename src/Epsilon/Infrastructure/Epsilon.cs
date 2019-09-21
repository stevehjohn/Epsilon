using System.Collections.Generic;
using Epsilon.Actors;
using Epsilon.Controls;
using Epsilon.Environment;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Infrastructure
{
    public class Epsilon : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly Map _map;
        private readonly MouseTracker _mouseTracker;
        private readonly List<IActor> _actors;

        private SpriteBatch _spriteBatch;

        public Epsilon()
        {
            _graphics = new GraphicsDeviceManager(this)
                        {
                            PreferredBackBufferWidth = Constants.ScreenBufferWidth,
                            PreferredBackBufferHeight = Constants.ScreenBufferHeight
                        };

            Content.RootDirectory = "_Content";

            _map = new Map();

            _mouseTracker = new MouseTracker();

            // TODO: Maybe use some assembly scanning technique to pick all IActors up...
            _actors = new List<IActor>
                      {
                          new Terrain(_map)
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
                var movement = _mouseTracker.GetMapMovement();

                _map.Move(movement);

                var heightManipulation = _mouseTracker.GetMouseHeightManipulation();

                // TODO: Yuck.
                var terrain = _actors[0] as Terrain;

                // ReSharper disable once PossibleNullReferenceException
                if (heightManipulation > 0 && terrain.SelectedTile != null)
                {
                    // TODO: Map manipulation within map class itself
                    _map.GetTile(terrain.SelectedTile.X, terrain.SelectedTile.Y).Height += heightManipulation;
                }
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
    }
}
