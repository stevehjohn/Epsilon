﻿using System.Collections.Generic;
using Epsilon.Actors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Infrastructure
{
    public class Epsilon : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly List<IActor> _actors;

        public Epsilon()
        {
            _graphics = new GraphicsDeviceManager(this)
                        {
                            PreferredBackBufferWidth = Constants.ScreenBufferWidth,
                            PreferredBackBufferHeight = Constants.ScreenBufferHeight
                        };

            Content.RootDirectory = "Content";

            _actors = new List<IActor>();

            // TODO: Maybe use some poncy assembly scanning technique to pick all these up...
            _actors.Add(new Terrain());
        }

        protected override void Initialize()
        {
            foreach (var actor in _actors)
            {
                actor.Initialise();
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (var actor in _actors)
            {
                actor.LoadContent();
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
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
