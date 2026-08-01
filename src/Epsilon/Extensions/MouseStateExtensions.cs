using System;
using Epsilon.Controls;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Extensions;

public static class MouseStateExtensions
{
    public static bool IsPressed(this MouseState mouseState, MouseButton mouseButton)
    {
        return mouseButton switch
        {
            MouseButton.Left => mouseState.LeftButton == ButtonState.Pressed,
            MouseButton.Right => mouseState.RightButton == ButtonState.Pressed,
            _ => throw new ArgumentOutOfRangeException(nameof(mouseButton), mouseButton, null)
        };
    }
}