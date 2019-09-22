using System;
using Epsilon.Infrastructure;
using Epsilon.Maths;

namespace Epsilon.Environment
{
    public class Map
    {
        private readonly Tile[,] _tiles;
        private readonly Random _rng;

        private int _rotation;
        public Coordinates Position;

        public int Rotation
        {
            get => _rotation;
            set
            {
                if (value < 0 || value > 270 || value % 90 != 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Rotation must be 0, 90, 180 or 270");
                }

                _rotation = value;
            }
        }

        public Map()
        {
            _tiles = new Tile[Constants.MapSize, Constants.MapSize];

            Position = new Coordinates(256, 256);

            _rng = new Random();

            InitialiseTerrainWithSimplexNoise();
        }

        public void Move(Direction direction)
        {
            switch (_rotation)
            {
                case 90:
                    Position = new Coordinates(Position.X - direction.Dy, Position.Y - direction.Dx);
                    break;
                case 180:
                    Position = new Coordinates(Position.X - direction.Dx, Position.Y + direction.Dy);
                    break;
                case 270:
                    Position = new Coordinates(Position.X + direction.Dy, Position.Y + direction.Dx);
                    break;
                default:
                    Position = new Coordinates(Position.X + direction.Dx, Position.Y - direction.Dy);
                    break;
            }
        }

        public Tile GetTile(int x, int y)
        {
            var tx = x;
            var ty = y;

            switch (_rotation)
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
                    throw new InvalidOperationException("Rotation is not a valid value, it should be 0, 90, 180 or 270");
            }

            return SafeGetTile(tx, ty);
        }

        public static TerrainType GetDefaultTerrainType(int height)
        {
            if (height < -4)
            {
                return TerrainType.Soil;
            }

            if (height < 10)
            {
                return TerrainType.Sand;
            }

            if (height < 15)
            {
                return TerrainType.Soil;
            }

            if (height < 40)
            {
                return TerrainType.Grass;
            }

            if (height < 57)
            {
                return TerrainType.Rock;
            }

            return TerrainType.Snow;
        }

        private Tile SafeGetTile(int x, int y)
        {
            x += Position.X;
            y += Position.Y;

            if (x < 0 || x >= Constants.MapSize || y < 0 || y >= Constants.MapSize)
            {
                return null;
            }

            return _tiles[x, y] ?? new Tile(0, TerrainType.Water);
        }

        private void InitialiseTerrainWithSimplexNoise()
        {
            var noise = SimplexNoise.Noise.Calc2D(Constants.MapSize, Constants.MapSize, 0.025f);

            for (var x = 0; x < Constants.MapSize; x++)
            {
                for (var y = 0; y < Constants.MapSize; y++)
                {
                    var height = TranslateNoiseToHeight(noise[x, y]);

                    // TODO: Magic numbers -1 and 3
                    var terrainType = GetDefaultTerrainType(height - 1 + _rng.Next(3));

                    // TODO: Magic numbers -1 and 3
                    var tile = new Tile(height, terrainType)
                               {
                                   IsEdge = x == 0 || y == 0 || x == Constants.MapSize - 1 || y == Constants.MapSize - 1,
                                   EdgeOffset = -1 + _rng.Next(3)
                               };

                    _tiles[x, y] = tile;

                    // TODO: Magic number 10
                    if (terrainType == TerrainType.Grass && _rng.Next(10) == 0)
                    {
                        tile.SceneryType = SceneryType.Tree;
                    }

                    // TODO: Magic number 200
                    if (terrainType == TerrainType.Grass && _rng.Next(200) == 0)
                    {
                        tile.SceneryType = SceneryType.Goat;
                    }

                    // TODO: Magic number 100
                    if (terrainType == TerrainType.Snow && _rng.Next(100) == 0)
                    {
                        tile.SceneryType = SceneryType.Snowman;
                    }

                    // TODO: Magic number 150
                    if (height < -10 && _rng.Next(150) == 0)
                    {
                        tile.SceneryType = SceneryType.Fish;
                    }
                }
            }
        }

        private static int TranslateNoiseToHeight(float noise)
        {
            return (int) (Constants.SeaFloor + noise / 255 * (Constants.MaxHeight + Math.Abs(Constants.SeaFloor)));
        }
    }
}