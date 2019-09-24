using Epsilon.Environment;
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

        public WaterFlow(Map map)
        {
            _map = map;
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
        }

        public float Render(float depth)
        {
            return depth;
        }
    }
}