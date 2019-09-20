using System;
using System.Collections.Generic;
using Epsilon.Extensions;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Controls
{
    public class MouseTracker
    {
        private readonly Dictionary<MouseButton, bool> _tracking;

        private readonly Dictionary<MouseButton, Coordinates> _previousCoordinates;

        public MouseTracker()
        {
            var buttons = (MouseButton[]) Enum.GetValues(typeof(MouseButton));

            _tracking = new Dictionary<MouseButton, bool>();
            _previousCoordinates = new Dictionary<MouseButton, Coordinates>();

            foreach (var button in buttons)
            {
                _tracking.Add(button, false);

                _previousCoordinates.Add(button, null);
            }
        }

        public Direction GetMapMovement()
        {
            var mouseState = Mouse.GetState();

            if (! Tracking(mouseState, MouseButton.Left))
            {
                return new Direction(0, 0);
            }

            var coordinates = GetMousePositionSeaLevel(mouseState);

            var direction = new Direction(_previousCoordinates[MouseButton.Left].X - coordinates.X, coordinates.Y - _previousCoordinates[MouseButton.Left].Y);

            _previousCoordinates[MouseButton.Left] = coordinates;

            return direction;
        }

        private bool Tracking(MouseState mouseState, MouseButton mouseButton)
        {
            if (! mouseState.IsPressed(mouseButton))
            {
                _tracking[mouseButton] = false;

                return false;
            }

            if (!_tracking[mouseButton])
            {
                _tracking[mouseButton] = true;

                _previousCoordinates[mouseButton] = GetMousePositionSeaLevel(mouseState);

                return false;
            }

            return true;
        }

        private static Coordinates GetMousePositionSeaLevel(MouseState mouseState)
        {
            var mouseX = (double) mouseState.X - (Constants.ScreenBufferWidth / 2 - Constants.TileWidthHalf) - Constants.TileWidthHalf;
            // TODO: Get rid of magic number 12. Where does it come from?
            var mouseY = (double) mouseState.Y - 12 - Constants.TileHeightHalf;

            var x = (int) Math.Floor((mouseX / Constants.TileWidthHalf + mouseY / Constants.TileHeightHalf) / 2);
            var y = (int) Math.Floor((mouseY / Constants.TileHeightHalf - mouseX / Constants.TileWidthHalf) / 2);

            return new Coordinates(x, y);
        }
    }
}