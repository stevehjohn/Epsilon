using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public class Terrain : IActor
    {
        private SpriteBatch _spriteBatch;
        private ContentManager _contentManager;
        private Texture2D _tiles;

        public void Initialise()
        {
        }

        public void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
        {
            _contentManager = contentManager;
            _spriteBatch = spriteBatch;

            _tiles = _contentManager.Load<Texture2D>("tile-set");
        }

        public void UpdateState()
        {
        }

        public float Render(float depth)
        {
            for (var x = 0; x < Constants.BoardSize; x++)
            {
                for (var y = 0; y < Constants.BoardSize; y++)
                {
                    var position = Translations.BoardToScreen(x, y);

                    // TODO: Stuff
                    _spriteBatch.Draw(_tiles, new Vector2(position.X, position.Y), new Rectangle(216, 0, Constants.TileSpriteWidth, Constants.TileSpriteHeight), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, depth); 

                    depth += Constants.DepthIncrement;
                }
            }

            return depth;
        }
    }
}