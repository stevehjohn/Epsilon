﻿namespace Epsilon.Infrastructure
{
    public static class Constants
    {
        public const int TileSpriteWidth = 54;
        public const int TileSpriteHeight = 30;

        public const int TileSpriteWidthHalf = 26;
        public const int TileSpriteHeightHalf = 15;

        public const int TileHeight = 27;
        public const int TileHeightHalf = 13;

        public const int BlockHeight = 3;

        public const int ScreenBufferWidth = 1280;
        public const int ScreenBufferHeight = 720;

        public const int YScreenOffset = ScreenBufferHeight / 2 - BoardSize * TileHeightHalf - 5;

        public const int MapSize = 512;

        public const int BoardSize = 25;

        public const float DepthIncrement = 0.00001f;
    }
}