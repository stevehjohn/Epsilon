namespace Epsilon.State
{
    public static class GameState
    {
        public static int WaterLevel { get; set; }

        static GameState()
        {
            WaterLevel = 0;
        }
    }
}