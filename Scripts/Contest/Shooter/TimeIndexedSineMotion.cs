using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class TimeIndexedSineMotion : IMotion
    {
        private float _frequency;
        public float Amplitude;
        public float Speed;
        private float _baseY;

        public TimeIndexedSineMotion(float speed, float baseY, float frequency, float amplitude)
        {
            _frequency = frequency;
            Amplitude = amplitude;
            Speed = speed;
            _baseY = baseY;
        }

        #region IMotion Members

        public void Update(GameObject gameObject, float deltaTime)
        {
            gameObject.Location.X += Speed * deltaTime;
            gameObject.Location.Y = _baseY + Math.Sin(ShooterLevel.Current.Ticks * _frequency) * Amplitude;
        }

        #endregion
    }
}
