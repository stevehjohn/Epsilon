using System.Collections.Generic;
using Epsilon.Environment;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public class WaterFlow : IActor
    {
        private readonly Map _map;

        private ContentManager _contentManager;
        private SpriteBatch _spriteBatch;
        private Texture2D _texture;

        private List<Glimmer> _glimmers;

        public WaterFlow(Map map)
        {
            _map = map;

            _glimmers = new List<Glimmer>();
        }

        public void Initialise()
        {
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _texture = _contentManager.Load<Texture2D>("glimmer");
        }

        public void UpdateState()
        {
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
            }

            toRemove.ForEach(g => _glimmers.Remove(g));
        }

        public float Render(float depth)
        {
            foreach (var glimmer in _glimmers)
            {
                var position = Translations.BoardToScreen(glimmer.BoardPosition.X, glimmer.BoardPosition.Y);

                _spriteBatch.Draw(_texture, new Vector2(position.X, position.Y), new Rectangle(0, 0, 1, 3), Color.White * glimmer.Alpha, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth);

                depth += Constants.DepthIncrement;
            }

            return depth;
        }
    }
}