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

            var coordinates = GetMousePositionSeaLevel(mouseState.X, mouseState.Y);

            var previousCoordinates = GetMousePositionSeaLevel(_previousCoordinates[MouseButton.Left]);

            var direction = new Direction(previousCoordinates.X - coordinates.X, coordinates.Y - previousCoordinates.Y);

            _previousCoordinates[MouseButton.Left] = new Coordinates(mouseState);

            return direction;
        }

        public int GetMouseHeightManipulation()
        {
            var mouseState = Mouse.GetState();

            if (! Tracking(mouseState, MouseButton.Right))
            {
                return 0;
            }

            var dy = _previousCoordinates[MouseButton.Right].Y - mouseState.Y;

            _previousCoordinates[MouseButton.Right] = new Coordinates(0, mouseState.Y + dy % Constants.BlockHeight);

            Console.WriteLine(dy / Constants.BlockHeight);

            return dy / Constants.BlockHeight;
        }

        private bool Tracking(MouseState mouseState, MouseButton mouseButton)
        {
            if (! mouseState.IsPressed(mouseButton))
            {
                _tracking[mouseButton] = false;

                return false;
            }

            if (! _tracking[mouseButton])
            {
                _tracking[mouseButton] = true;

                _previousCoordinates[mouseButton] = new Coordinates(mouseState);

                return false;
            }

            return true;
        }

        private static Coordinates GetMousePositionSeaLevel(Coordinates coordinates)
        {
            return GetMousePositionSeaLevel(coordinates.X, coordinates.Y);
        }

        private static Coordinates GetMousePositionSeaLevel(int x, int y)
        {
            var mouseX = (double) x - (Constants.ScreenBufferWidth / 2 - Constants.TileWidthHalf) - Constants.TileWidthHalf;
            // TODO: Get rid of magic number 12. Where does it come from?
            var mouseY = (double) y - 12 - Constants.TileHeightHalf;

            var px = (int) Math.Floor((mouseX / Constants.TileWidthHalf + mouseY / Constants.TileHeightHalf) / 2);
            var py = (int) Math.Floor((mouseY / Constants.TileHeightHalf - mouseX / Constants.TileWidthHalf) / 2);

            return new Coordinates(px, py);
        }
    }
}