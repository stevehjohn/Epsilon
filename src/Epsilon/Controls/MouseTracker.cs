﻿using System;
using Epsilon.Infrastructure;
using Epsilon.Maths;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Controls
{
    public class MouseTracker
    {
        private bool _tracking;

        private Coordinates _previousCoordinates;

        public Direction GetMovement()
        {
            var state = Mouse.GetState();

            if (state.LeftButton != ButtonState.Pressed)
            {
                _tracking = false;

                return new Direction(0, 0);
            }

            if (! _tracking)
            {
                _tracking = true;

                _previousCoordinates = GetMousePositionSeaLevel(state);

                return new Direction(0, 0);
            }

            var coordinates = GetMousePositionSeaLevel(state);

            var direction = new Direction(_previousCoordinates.X - coordinates.X, coordinates.Y - _previousCoordinates.Y);

            _previousCoordinates = coordinates;

            return direction;
        }

        public static Coordinates GetMousePositionSeaLevel(MouseState mouseState)
        {
            // TODO: Get rid of magic numbers!
            var mouseX = (double) mouseState.X - 622 - Constants.TileWidthHalf;
            var mouseY = (double) mouseState.Y - 26 - Constants.TileHeightHalf;

            // TODO: Fix above coords rather than adding 1 here.
            var x = (int) Math.Floor((mouseX / Constants.TileWidthHalf + mouseY / Constants.TileHeightHalf) / 2) + 1;
            var y = (int) Math.Floor((mouseY / Constants.TileHeightHalf - mouseX / Constants.TileWidthHalf) / 2) + 1;

            return new Coordinates(x, y);
        }
    }
}