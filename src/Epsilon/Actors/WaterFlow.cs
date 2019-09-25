using System;
using System.Collections.Generic;
using Epsilon.Environment;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Epsilon.State;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public class WaterFlow : IActor
    {
        private readonly Map _map;
        private readonly List<Glimmer> _glimmers;
        private readonly Random _rng;

        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;
        private List<Coordinates> _edgeTiles;


        public WaterFlow(Map map)
        {
            _map = map;

            _glimmers = new List<Glimmer>();

            _rng = new Random();
        }

        public void Initialise()
        {
            _edgeTiles = new List<Coordinates>();

            for (var x = 0; x < Constants.MapSize; x++)
            {
                for (var y = 0; y < Constants.MapSize; y++)
                {
                    var tile = _map.GetMapTile(x, y);

                    if (tile == null || ! tile.IsEdge)
                    {
                        continue;
                    }

                    _edgeTiles.Add(new Coordinates(x, y));
                }
            }
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _texture = _contentManager.Load<Texture2D>("glimmer");
        }

        public void UpdateState()
        {
            foreach (var edgeTile in _edgeTiles)
            {
                var tile = _map.GetMapTile(edgeTile.X, edgeTile.Y);

                if (GameState.WaterLevel > tile.Height && _rng.Next(10) == 0)
                {
                    _glimmers.Add(new Glimmer
                                  {
                                      Alpha = 0.0f,
                                      AlphaDelta = 0.025f,
                                      BoardPosition = edgeTile,
                                      YOffset = _rng.Next(Constants.ScreenBufferHeight / 2),
                                      Velocity = _rng.Next(10) / 10.0f,
                                      XOffset = _rng.Next(Constants.TileSpriteWidth)
                                  });
                }
            }

            var toRemove = new List<Glimmer>();

            foreach (var glimmer in _glimmers)
            {
                glimmer.Alpha += glimmer.AlphaDelta;

                if (glimmer.Alpha > 1.0f)
                {
                    glimmer.AlphaDelta = -glimmer.AlphaDelta;
                    glimmer.Alpha += glimmer.AlphaDelta;
                }

                if (glimmer.Alpha <= 0)
                {
                    toRemove.Add(glimmer);
                }

                glimmer.YOffset += glimmer.Velocity;
            }

            toRemove.ForEach(g => _glimmers.Remove(g));
        }

        public float Render(float depth)
        {
            var origin = _map.GetOrigin();

            foreach (var glimmer in _glimmers)
            {
                var position = Translations.BoardToScreen(glimmer.BoardPosition.X - origin.X, glimmer.BoardPosition.Y - origin.Y);

                _spriteBatch.Draw(_texture, new Vector2(position.X + glimmer.XOffset, position.Y + glimmer.YOffset), new Rectangle(0, 0, 1, 3), Color.White * glimmer.Alpha, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);

                depth += Constants.DepthIncrement;
            }

            return depth;
        }
    }
}