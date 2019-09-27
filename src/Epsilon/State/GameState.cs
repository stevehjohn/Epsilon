using Epsilon.Infrastructure;

namespace Epsilon.State
{
    public static class GameState
    {
        private static int _waterLevel;

        public static int WaterLevel
        {
            get => _waterLevel;
            set
            {
                if (value < Constants.SeaFloor)
                {
                    _waterLevel = Constants.SeaFloor;
                    return;
                }

                if (value > Constants.MaxWaterHeight)
                {
                    _waterLevel = Constants.MaxWaterHeight;
                    return;
                }

                _waterLevel = value;
            }
        }


        public static float Scale { get; set; }

        static GameState()
        {
            WaterLevel = 0;
            Scale = 0.5f;
        }
    }
}