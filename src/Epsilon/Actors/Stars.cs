using System;
using System.Collections.Generic;
using Epsilon.Coordination;
using Epsilon.Environment;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public class Stars : IActor
    {
        private readonly Map _map;

        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private List<Star> _stars;

        public Stars(EventManager eventManager, Map map)
        {
            _map = map;

            eventManager.AddSubscription(EventType.MapMoved, direction => MapMoved((Direction) direction));
        }

        public void Initialise()
        {
            _stars = new List<Star>();

            var rng = new Random();

            for (var i = 0; i < Constants.NumberOfStars; i++)
            {
                var star = new Star
                           {
                               AnchorX = rng.Next(Constants.ScreenBufferWidth),
                               AnchorY = rng.Next(Constants.ScreenBufferHeight / 3),
                               Type = rng.Next(1),
                               Velocity = rng.Next(10) * 0.2f,
                               Intensity = 0.5f + rng.Next(50) / 100.0f
                           };

                star.X = star.AnchorX;
                star.Y = star.AnchorY;

                switch (rng.Next(3))
                {
                    case 0:
                        star.Color = Color.White;
                        break;
                    case 1:
                        star.Color = Color.LightBlue;
                        break;
                }

                _stars.Add(star);
            }
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager; 
            _spriteBatch = spriteBatch;

            _texture = _contentManager.Load<Texture2D>("stars");
        }

        public void UpdateState()
        {
        }

        public void MapMoved(Direction direction)
        {
            foreach (var star in _stars)
            {
                var position = _map.Position;

                star.X = (star.AnchorX - (position.X - position.Y) * star.Velocity) % Constants.ScreenBufferWidth;
                star.Y = (star.AnchorY - (position.X + position.Y) * star.Velocity) % Constants.ScreenBufferWidth;
            }
        }

        public float Render(float depth)
        {
            foreach (var star in _stars)
            {
                // TODO: Magic numbers
                _spriteBatch.Draw(_texture, new Vector2(star.X, star.Y), new Rectangle(7 * star.Type, 0, 7, 7), star.Color * star.Intensity, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);

                depth += Constants.DepthIncrement;
            }

            return depth;
        }
    }
}