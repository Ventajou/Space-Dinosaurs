using System;
using System.Runtime.CompilerServices;

namespace Vtj.Contest.Race
{
    internal class FlakObject
    {
        private const int LifeSpan = 4000;

        public float X;
        public float Y;
        private DateTime _startDate;
        private float _incX;
        private float _incY;
        private int _delay;

        public FlakObject(float angle, float x, int delay)
        {
            _startDate = DateTime.Now;
            X = x;
            Y = 320;
            _incX = Math.Cos(angle);
            _incY = Math.Sin(angle);
            _delay = delay;
        }

        public bool Update()
        {
            long delta = DateTime.Now - _startDate;

            if (delta > _delay)
            {
                X += _incX;
                Y += _incY;
            }

            if (delta > LifeSpan + _delay) return true;
            return false;
        }
    }
}
