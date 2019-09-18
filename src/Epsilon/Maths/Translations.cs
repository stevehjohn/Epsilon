using Epsilon.Infrastructure;

namespace Epsilon.Maths
{
    public static class Translations
    {
        public static Coordinates BoardToScreen(int x, int y)
        {
            var sx = Constants.ScreenBufferWidth / 2 + x * Constants.TileSpriteWidth - Constants.ScreenBufferWidth / 2;

            var sy = 0;

            return new Coordinates(sx, sy);
        }
    }
}