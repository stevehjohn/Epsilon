namespace Epsilon.Infrastructure
{
    public static class Constants
    {
        public const int TileSpriteWidth = 38;
        public const int TileSpriteHeight = 22;

        public const int TileSpriteWidthHalfWithOverlap = 18;
        public const int TileSpriteWidthHalf = 19;

        public const int TileHeight = 19;
        public const int TileHeightHalf = 9;
        public const int TileWidthHalf = 18;

        public const int ScenerySpriteHeight = 60;
        public const int ScenerySpriteWidth = 38;

        public const int SkySpriteHeight = 280;
        public const int SkySpriteWidth = 38;

        public const int BlockHeight = 3;

        public const int ScreenBufferWidth = 1280;
        public const int ScreenBufferHeight = 720;

        public const int YScreenOffset = ScreenBufferHeight / 2 - BoardSize * TileHeightHalf;

        public const int MapSize = 170;
        public const int MapSizeHalf = 85;

        public const float RadiansResolution = 0.001f;
        public const float RadiansHighResolution = 0.00001f;

        public const int BoardSize = 38;
        public const int BoardOverrun = 20;

        public const float DepthIncrement = 0.00001f;

        public const int SeaFloor = -20;
        public const int MaxHeight = 75;

        public const int NumberOfStars = 400;
    }
}