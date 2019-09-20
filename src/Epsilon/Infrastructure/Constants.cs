namespace Epsilon.Infrastructure
{
    public static class Constants
    {
        public const int TileSpriteWidth = 38;
        public const int TileSpriteHeight = 22;

        public const int TileSpriteWidthHalf = 18;

        public const int TileHeight = 19;
        public const int TileHeightHalf = 9;
        public const int TileWidthHalf = 18;

        public const int BlockHeight = 3;

        public const int ScreenBufferWidth = 1280;
        public const int ScreenBufferHeight = 720;

        public const int YScreenOffset = ScreenBufferHeight / 2 - BoardSize * TileHeightHalf;

        public const int MapSize = 512;

        public const int BoardSize = 38;

        public const float DepthIncrement = 0.00001f;

        public const int SeaFloor = -20;
    }
}