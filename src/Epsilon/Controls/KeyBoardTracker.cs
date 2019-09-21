using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Epsilon.Controls
{
    public class KeyBoardTracker
    {
        private readonly Queue<Keys> _keyPresses;

        private List<Keys> _previouslyPressed;

        public KeyBoardTracker()
        {
            _keyPresses = new Queue<Keys>();

            _previouslyPressed = new List<Keys>();
        }

        public void TrackState()
        {
            var pressed = Keyboard.GetState().GetPressedKeys().ToList();

            var unpressed = _previouslyPressed.Except(pressed);

            foreach (var key in unpressed)
            {
                _keyPresses.Enqueue(key);
            }

            _previouslyPressed = pressed;
        }

        public Keys? GetKeyPress()
        {
            return _keyPresses.Count > 0
                       ? _keyPresses.Dequeue()
                       : (Keys?) null;
        }
    }
}