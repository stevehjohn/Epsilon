using Epsilon.Infrastructure;
using Epsilon.Maths;

namespace Epsilon.Environment
{
    public class Map
    {
        private readonly Coordinates _position;

        private readonly Tile[,] _tiles;

        public Map()
        {
            _tiles = new Tile[Constants.MapSize, Constants.MapSize];

            _position = new Coordinates(0, 0);

            InitialiseTerrain();
        }

        private void InitialiseTerrain()
        {
            _tiles[0, 0] = new Tile(0, TerrainType.Grass);
            _tiles[2, 2] = new Tile(0, TerrainType.Sand);
            _tiles[2, 0] = new Tile(0, TerrainType.Rock);
            _tiles[0, 2] = new Tile(0, TerrainType.Soil);

            _tiles[4, 4] = new Tile(0, TerrainType.Sand);
            _tiles[4, 5] = new Tile(0, TerrainType.Sand);
            _tiles[4, 6] = new Tile(0, TerrainType.Sand);
            _tiles[5, 4] = new Tile(0, TerrainType.Sand);
            _tiles[5, 5] = new Tile(1, TerrainType.Sand);
            _tiles[5, 6] = new Tile(0, TerrainType.Sand);
            _tiles[6, 4] = new Tile(0, TerrainType.Sand);
            _tiles[6, 5] = new Tile(0, TerrainType.Sand);
            _tiles[6, 6] = new Tile(0, TerrainType.Sand);

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

        public Tile GetTile(Coordinates coordinates)
        {
            return GetTile(coordinates.X, coordinates.Y);
        }
    }
}