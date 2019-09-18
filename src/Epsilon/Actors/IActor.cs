using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Epsilon.Actors
{
    public interface IActor
    {
        void Initialise();

        void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch);

        void UpdateState();

        float Render(float depth);
    }
}