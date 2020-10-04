using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Collection_of_Useful_Code_Bases__Some_Monogame_
{
    /// <summary>
    /// Implements a position, supporting two coordinates (two-dimensional: x and y),
    /// in addition to an offset for x / y, which is equivalent to the Width and Height.
    /// Also has some basic collision detection with ContainsPosition().
    /// </summary>
    class Position
    {
        private Vector2 _topleft;
        private Vector2 _offsets;
        /// <summary>
        /// Returns the offset (difference between the topleft and bottomright co-ordinates
        /// as a Vector2. Use Width, Height properties if dealing with the individual components.
        /// </summary>
        public Vector2 Offset { get => _offsets; private set => _offsets = value; }
        /// <summary>
        /// Returns the Width (Offset.X) of the Position. 
        /// </summary>
        public float Width { get => Offset.X; private set => _offsets.X = value; }
        /// <summary>
        /// Returns the Height (Offset.Y) of the Position.
        /// </summary>
        public float Height { get => Offset.Y; private set => _offsets.Y = value; }

        /// <summary>
        /// Returns the position in the top-left as a Vector2.
        /// </summary>
        public Vector2 TopLeft
        {
            get { return _topleft; }
            private set { _topleft = value; }
        }
        public float TopLeftX { get => TopLeft.X; }
        public float TopLeftY { get => TopLeft.Y; }
        public float BottomRightX { get => BottomRight.X; }
        public float BottomRightY { get => BottomRight.Y; }

        /// <summary>
        /// Returns the position in the bottom-right as a Vector2.
        /// </summary>
        public Vector2 BottomRight
        { // Note: DO NOT define a set accessor for this.
            get { return Vector2.Add(TopLeft, Offset); }
        }

        /// <summary>
        /// Allows for easy collision detection. Returns true if the position
        /// parameter is within the borders of this Position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool ContainsPosition(Vector2 position)
        {
            if ((TopLeft.X < position.X && position.X < BottomRight.X) &&
                (TopLeft.Y < position.Y && position.Y < BottomRight.Y))
                return true;
            else return false;
        }
        /// <summary>
        /// Allows for easy collision detection. Returns true if any part
        /// of this Position class and the Position argument are overlapping.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool ContainsPosition(Position position)
        {
            // TODO: implement. 

            throw new NotImplementedException();
        }

        /// <summary>
        /// Takes a Vector2, and uses this to move the Position.
        /// Does not perform any form of bounds checks.
        /// </summary>
        /// <param name="movement"></param>
        public void Move (Vector2 movement)
        {
            TopLeft = Vector2.Add(TopLeft, movement);
        }

        /// <summary>
        /// Takes two floats, x and y, and uses these to move the position.
        /// Does not perform any form of bounds checks.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Move(float x = 0, float y = 0)
        { 
            // Note: do not modify. Any changes to the Move method should be done on Move(Vector2) not this Move(float, float).
            Move(new Vector2(x, y));
        }

        /// <summary>
        /// Resizes the Position object, but does not affect the TopLeft position.
        /// </summary>
        /// <param name="newSize"></param>
        public void Resize (Vector2 newSize)
        {
            Offset = newSize;
        }
        /// <summary>
        /// Takes two (nullable) floats to replace the old values of Offset (Width, and Height).
        /// If the value is null, no change occurs to that part of the Offset.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Resize (float? x = null, float? y = null)
        {
            // If parameter left unspecified (null) then defaults to current size.
            // This prevents null from being a possible value of x or y.
            x = x ?? Width;
            y = x ?? Height;

            // Will always have a proper value, and not be null, due to above code.
            Resize(new Vector2(x.Value, y.Value));
        }

        /// <summary>
        /// Multiplies the Offset (Width, Height) by the given multiplier. Defaults to 1 which results in no change.
        /// </summary>
        /// <param name="multiplier"></param>
        public void ResizeByMultiple (float multiplier = 1, bool applyToX = true, bool applyToY = true)
        {
            if (multiplier <= 0) throw new ArgumentOutOfRangeException("Multiplier must be greater than zero");

            if (applyToX) Width = Width * multiplier;
            if (applyToY) Height = Height * multiplier;
        }

        public static implicit operator Vector2(Position rhs)
        {
            return rhs.TopLeft;
        }

        public static implicit operator Position(Vector2 rhs)
        { // Convert Vector2 into position; Width and Height are given as 0.
            return new Position(rhs.X, rhs.Y, 0, 0);
        }


        public override string ToString()
        {
            return $"({TopLeftX}, {TopLeftY}, {BottomRightX}, {BottomRightY})";
        }


        public Position (Vector2 topLeft, Vector2 offset)
        {
            TopLeft = topLeft;
            Offset = offset;
        }

        public Position(float topLeftX, float topLeftY, float offsetX, float offsetY)
            : this(new Vector2(topLeftX, topLeftY), new Vector2(offsetX, offsetY)) { }
    }

    
}
