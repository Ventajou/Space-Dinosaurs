using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class StraightMotion : IMotion
    {
        private Vector2D _motionVector;

        public StraightMotion(Vector2D motionVector)
        {
            _motionVector = motionVector.Clone();
        }

        #region IMotion Members

        public void Update(GameObject gameObject, float deltaTime)
        {
            gameObject.Location.TranslateByCoordinates((_motionVector.X) * deltaTime, _motionVector.Y * deltaTime);
        }

        #endregion
    }
}
