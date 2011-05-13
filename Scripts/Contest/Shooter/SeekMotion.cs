using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    internal class SeekMotion : IMotion
    {
        public GameObject Target;
        public double Angle;
        
        private Vector2D _lastDirection;
        private float _speed;
        private float _rotationSpeed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeekMotion"/> class.
        /// </summary>
        /// <param name="target">The target object.</param>
        /// <param name="speed">The speed (under 0.1f is better).</param>
        /// <param name="defaultVector">The default vector used when no target is set yet.</param>
        /// <param name="rotationSpeed">The rotation speed.</param>
        public SeekMotion(GameObject target, float speed, Vector2D defaultVector, float rotationSpeed)
        {
            Target = target;
            _lastDirection = defaultVector;
            _speed = speed;
            _rotationSpeed = rotationSpeed;
        }
        #region IMotion Members

        public void Update(GameObject gameObject, float deltaTime)
        {
            if (!Script.IsNullOrUndefined(Target))
            {
                float lastAngle = _lastDirection.GetAbsoluteAngle();
                Vector2D targetVector = new Vector2D(Target.Location.X - gameObject.Location.X, Target.Location.Y - gameObject.Location.Y);

                double relativeAngle = targetVector.GetAbsoluteAngle() - lastAngle;

                if (Math.Abs(relativeAngle) > Math.PI)
                {
                    if (relativeAngle < 0) relativeAngle += Math.PI * 2;
                    else relativeAngle -= Math.PI * 2;
                }

                if (relativeAngle != 0)
                {
                    Angle = lastAngle + (Math.Min(_rotationSpeed, Math.Abs(relativeAngle)) * (relativeAngle / Math.Abs(relativeAngle)));
                    _lastDirection.X = _speed * Math.Cos(Angle);
                    _lastDirection.Y = _speed * Math.Sin(Angle);
                }
            }

            gameObject.Location.TranslateByVector(_lastDirection);
        }

        #endregion
    }
}
