using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Controls
{
    public class KeyboardTracker
    {
        private readonly Queue<Keys> _keyPresses;

        private List<Keys> _previouslyPressed;

        public bool Ctrl { get; set; }

        public KeyboardTracker()
        {
            _keyPresses = new Queue<Keys>();

            _previouslyPressed = new List<Keys>();
        }

        public void TrackState()
        {
            var state = Keyboard.GetState();

            var pressed = state.GetPressedKeys().ToList();

            var unpressed = _previouslyPressed.Except(pressed);

            foreach (var key in unpressed)
            {
                _keyPresses.Enqueue(key);
            }

            _previouslyPressed = pressed;

            Ctrl = state.IsKeyDown(Keys.LeftControl);
        }

        public Keys? GetKeyPress()
        {
            return _keyPresses.Count > 0
                       ? _keyPresses.Dequeue()
                       : (Keys?) null;
        }
    }
}