namespace Epsilon.Environment
{
    public class Tile
    {
        public int Height { get; set; }

        public TerrainType TerrainType { get; set; }

        public Tile(int height, TerrainType terrainType)
        {
            Height = height;
            TerrainType = terrainType;
        }
    }
}