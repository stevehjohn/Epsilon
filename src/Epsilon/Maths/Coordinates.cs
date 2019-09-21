using Microsoft.Xna.Framework.Input;

namespace Epsilon.Maths
{
    public class Coordinates
    {
        public int X { get; }

        public int Y { get; }

        public Coordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Coordinates(MouseState mouseState)
        {
            X = mouseState.X;
            Y = mouseState.Y;
        }
    }
}