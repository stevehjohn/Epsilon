using Epsilon.Infrastructure;

namespace Epsilon.Environment
{
    public class WorldMap
    {
        private Tile[,] _tiles;

        public WorldMap()
        {
            _tiles = new Tile[Constants.MapSize, Constants.MapSize];
        }
    }
}