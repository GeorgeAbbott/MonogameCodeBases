using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace Collection_of_Useful_Code_Bases__Some_Monogame_
{
    class InputHandler
    {
        private KeyboardState _prevState;
        public KeyboardState PreviousState { get => _prevState; set => _prevState = value; }

        private KeyboardState _currentState;
        public KeyboardState CurrentState { get => _currentState; set => _currentState = value; }

        public void ReadInput()
        {
            PreviousState = CurrentState;
            CurrentState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            if (CurrentState[key] == KeyState.Down) return true;
            else return false;
        }


        private DateTime _isKeyDownLastCheck = DateTime.Now;
        public DateTime KeyDownLastCheck
        {
            get { return _isKeyDownLastCheck; }
            private set { _isKeyDownLastCheck = DateTime.Now; }
        }



        /// <summary>
        /// Checks if a key is down, and will return false if the last keypress was earlier than
        /// the timegap parameter. Keep changeKeyDownLastCheckIfFalse as false,
        /// otherwise calling this function will interrupt the checking process.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timegap"></param>
        /// <param name="changeKeyDownLastCheckIfFalse"></param>
        /// <returns></returns>
        public bool IsKeyDown(Keys key, double timegap, bool changeKeyDownLastCheckIfFalse = false)
        {
            // Takes a DateTime, will return false if the DateTime is not after the last check.
            // If enough time has passed, it will perform the check and return either true or false.
            // Will not change KeyDownLastCheck unless true returned, except if changeKeyDownLastCheckIfFalse is true.

            if (!((DateTime.Now - KeyDownLastCheck).TotalSeconds > timegap))
            { // if not enough time has passed since last check
                if (changeKeyDownLastCheckIfFalse) KeyDownLastCheck = DateTime.Now;
                return false;
            }
            else // enough time has passed
            {
                if (CurrentState[key] == KeyState.Down)
                { // Key has been pressed
                    KeyDownLastCheck = DateTime.Now;
                    return true;
                }
                else
                { // Key has not been pressed
                    if (changeKeyDownLastCheckIfFalse)
                    {
                        KeyDownLastCheck = DateTime.Now;
                        return false;
                    }
                    else return false;
                }
            }





        }
        /// <summary>
        /// Checks if a key has just been pressed, 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool OnKeyPress(Keys key)
        {
            if (CurrentState[key] == KeyState.Down && PreviousState[key] == KeyState.Up) return true;
            else return false;
        }

        public bool OnKeyRelease(Keys key)
        {
            if (CurrentState[key] == KeyState.Up && PreviousState[key] == KeyState.Down) return true;
            else return false;
        }
        /// <summary>
        /// Calls one of the methods in this class for every key listed in keys;
        /// will return true if at least one of these method calls returns true.
        /// </summary>
        /// <param name="keys"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public bool ForAnyKey(List<Keys> keys, Func<Keys, bool> func)
        {
            foreach (Keys key in keys)
                if (func(key)) return true;

            return false;
        }
        /// <summary>
        /// Calls one of the methods in this class for every key listed in keys;
        /// only if all method calls return true will this method return true.
        /// </summary>
        /// <param name="keys"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public bool ForAllKeys(List<Keys> keys, Func<Keys, bool> func)
        {
            foreach (Keys key in keys)
                if (!func(key)) return false;

            return true;
        }

    }
}
