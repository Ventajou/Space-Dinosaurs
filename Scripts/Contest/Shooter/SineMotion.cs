using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    internal class SineMotion : IMotion
    {
        private float _frequency;
        private float _amplitude;
        private float _speed;
        private float _baseY;

        public SineMotion(float speed, float baseY, float frequency, float amplitude)
        {
            _frequency = frequency;
            _amplitude = amplitude;
            _speed = speed;
            _baseY = baseY;
        }

        #region IMotion Members

        public void Update(GameObject gameObject, float deltaTime)
        {
            gameObject.Location.X += _speed * deltaTime;
            gameObject.Location.Y = (_baseY - _amplitude) + Math.Sin(gameObject.Location.X * _frequency) * _amplitude;
        }

        #endregion
    }
}
