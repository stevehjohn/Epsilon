using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public class Terrain : IActor
    {
        private readonly ContentManager _contentManager;

        private Texture2D _tiles;

        public Terrain(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public void Initialise()
        {
        }

        public void LoadContent()
        {
            _tiles = _contentManager.Load<Texture2D>("tile-set");
        }

        public void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public float Render(float depth)
        {
            throw new System.NotImplementedException();
        }
    }
}