namespace Epsilon.State;

public static class GameState
{
    public static int WaterLevel { get; set; }

    public static int Brightness { get; }

    static GameState()
    {
        WaterLevel = 0;
        Brightness = 255;
    }
}