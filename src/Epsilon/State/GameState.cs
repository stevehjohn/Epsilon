namespace Epsilon.State
{
    public static class GameState
    {
        public static int WaterLevel { get; set; }

        public static int Brightness { get; set; }

        static GameState()
        {
            WaterLevel = 0;
            Brightness = 255;
        }
    }
}