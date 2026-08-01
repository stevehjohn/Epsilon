namespace Epsilon.Environment;

public class Tile
{
    public int Height { get; set; }

    public TerrainType? TerrainType { get; set; }

    public SceneryType? SceneryType { get; set; }

    public bool IsEdge { get; init; }

    public int EdgeOffset { get; init; }

    public Tile(int height, TerrainType? terrainType = null)
    {
        Height = height;
        TerrainType = terrainType;
    }
}