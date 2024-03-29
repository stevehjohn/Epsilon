﻿namespace Epsilon.Environment
{
    public class Tile
    {
        public int Height { get; set; }

        public TerrainType? TerrainType { get; set; }

        public SceneryType? SceneryType { get; set; }

        public bool IsEdge { get; set; }

        public int EdgeOffset { get; set; }

        public Tile(int height, TerrainType? terrainType = null)
        {
            Height = height;
            TerrainType = terrainType;
        }
    }
}