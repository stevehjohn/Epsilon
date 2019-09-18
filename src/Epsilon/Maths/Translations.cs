using Epsilon.Infrastructure;

namespace Epsilon.Maths
{
    public static class Translations
    {
        public static Coordinates BoardToScreen(int x, int y)
        {
            var sx = Constants.ScreenBufferWidth / 2 + (x - y) * (Constants.TileSpriteWidthHalf) - Constants.TileSpriteWidthHalf;

            var sy = Constants.YScreenOffset + y * Constants.TileHeightHalf + x * Constants.TileHeightHalf;

            return new Coordinates(sx, sy);
        }
    }
}