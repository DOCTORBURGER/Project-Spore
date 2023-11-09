﻿using Microsoft.Xna.Framework.Input;

namespace Project_Spore.State_Management
{
    public class InputAction
    {
        private readonly Keys[] _keys;
        private readonly bool _firstPressOnly;

        // This maps to the method in InputState
        private delegate bool KeyPress(Keys key);

        public InputAction(Keys[] triggerKeys, bool firstPressOnly)
        {
            _keys = triggerKeys != null ? triggerKeys.Clone() as Keys[] : new Keys[0];
            _firstPressOnly = firstPressOnly;
        }

        public bool Occurred(InputState input)
        {
            KeyPress keyPress;

            if (_firstPressOnly)
            {
                keyPress = input.IsNewKeyPress;
            }
            else
            {
                keyPress = input.IsKeyPressed;
            }

            foreach (Keys key in _keys)
            {
                if (keyPress(key)) return true;
            }

            return false;
        }
    }
}
