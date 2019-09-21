using System;
using Epsilon.Infrastructure;
using Epsilon.Maths;

namespace Epsilon.Environment
{
    public class Map
    {
        private readonly Tile[,] _tiles;

        private int _rotation;

        public Coordinates Position;

        public int Rotation
        {
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

            Position = new Coordinates(90, 90);

            InitialiseTerrain();
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

        private void InitialiseTerrain()
        {
            for (var x = 0; x < Constants.MapSize; x++)
            {
                for (var y = 0; y < Constants.MapSize; y++)
                {
                    _tiles[x, y] = new Tile(Constants.SeaFloor);
                }
            }
        }
    }
}