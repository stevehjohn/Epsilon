﻿using System;
using Epsilon.Infrastructure;
using Epsilon.Maths;

namespace Epsilon.Environment
{
    public class Map
    {
        private readonly Tile[,] _tiles;

        private Coordinates _position;

        public Map()
        {
            _tiles = new Tile[Constants.MapSize, Constants.MapSize];

            _position = new Coordinates(100, 100);

            InitialiseTerrain();
        }

        public Tile GetTile(int x, int y)
        {
            x += _position.X;
            y += _position.Y;

            if (x < 0 || x >= Constants.MapSize || y < 0 || y >= Constants.MapSize)
            {
                return null;
            }

            return _tiles[x, y] ?? new Tile(0, TerrainType.Water);
        }

        public void Move(Direction direction)
        {
            _position = new Coordinates(_position.X + direction.Dx, _position.Y - direction.Dy);
        }

        public Tile GetTile(int x, int y, int rotation)
        {
            var tx = x;
            var ty = y;

            switch (rotation)
            {
                case 0:
                    break;
                case 90:
                    tx = y;
                    ty = Constants.BoardSize - 1 - x;
                    break;
                case 180:
                    tx = Constants.BoardSize - 1 - x;
                    ty = Constants.BoardSize - 1 - y;
                    break;
                case 270:
                    tx = Constants.BoardSize - 1 - y;
                    ty = x;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), "Invalid value for rotation, must be 0, 90, 180 or 270");
            }

            return GetTile(tx, ty);
        }

        private void InitialiseTerrain()
        {
            for (var x = 0; x < Constants.MapSize; x++)
            {
                for (var y = 0; y < Constants.MapSize; y++)
                {
                    _tiles[x, y] = new Tile(-5, TerrainType.Soil);
                }
            }

            _tiles[100, 100] = new Tile(0, TerrainType.Grass);
            _tiles[102, 102] = new Tile(0, TerrainType.Sand);
            _tiles[102, 100] = new Tile(0, TerrainType.Rock);
            _tiles[100, 102] = new Tile(0, TerrainType.Soil);

            _tiles[104, 104] = new Tile(2, TerrainType.Sand);
            _tiles[104, 105] = new Tile(1, TerrainType.Sand);
            _tiles[104, 106] = new Tile(0, TerrainType.Sand);
            _tiles[105, 104] = new Tile(1, TerrainType.Sand);
            _tiles[105, 105] = new Tile(0, TerrainType.Sand);
            _tiles[105, 106] = new Tile(-1, TerrainType.Sand);
            _tiles[106, 104] = new Tile(0, TerrainType.Sand);
            _tiles[106, 105] = new Tile(-1, TerrainType.Sand);
            _tiles[106, 106] = new Tile(-2, TerrainType.Sand);
        }
    }
}