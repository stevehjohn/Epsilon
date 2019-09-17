using Epsilon.Infrastructure;
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
        }

        public float Render(float depth)
        {
            for (var y = 0; y < Constants.BoardSize; y++)
            {
                for (var x = 0; x < Constants.BoardSize; x++)
                {
                }
            }

            return depth;
        }
    }
}