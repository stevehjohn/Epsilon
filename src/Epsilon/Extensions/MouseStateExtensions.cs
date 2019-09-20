using System;
using Epsilon.Controls;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Extensions
{
    public static class MouseStateExtensions
    {
        public static bool IsPressed(this MouseState mouseState, MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    return mouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Right:
                    return mouseState.RightButton == ButtonState.Pressed;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, null);
            }
        }
    }
}