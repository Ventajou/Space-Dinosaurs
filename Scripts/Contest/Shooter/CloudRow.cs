using System.Html;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    [Imported]
    internal class CloudRow
    {
        public ImageElement Image;
        public float OffsetX;
        public float OffsetY;
        public float RelativeSpeed;
        public List<GameObject> Objects;
    }
}
