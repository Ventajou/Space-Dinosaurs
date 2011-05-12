using System;
using System.Collections.Generic;

namespace Vtj.Gaming
{
    internal class Vector2D
    {
        public float X;
        public float Y;

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D TranslateByVector(Vector2D vector)
        {
            X += vector.X;
            Y += vector.Y;
            return this;
        }

        public Vector2D SetOrigin(Vector2D origin)
        {
            X -= origin.X;
            Y -= origin.Y;
            return this;
        }

        public Vector2D TranslateByCoordinates(float x, float y)
        {
            X += x;
            Y += y;
            return this;
        }

        public float GetAbsoluteAngle()
        {
            return Math.Atan2(Y, X);
        }

        public Vector2D Clone()
        {
            return new Vector2D(X, Y);
        }
    }
}
