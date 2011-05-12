using System;
using System.Collections.Generic;

namespace Vtj.Gaming
{
    internal class Vector3D : Vector2D
    {
        public float Z;

        public Vector3D(float x, float y, float z) : base(x, y)
        {
            Z = z;
        }
    }
}
