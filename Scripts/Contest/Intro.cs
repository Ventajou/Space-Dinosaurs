using Vtj.Gaming;
using System.Runtime.CompilerServices;

namespace Vtj.Contest
{
    internal class Intro : Scene
    {
        public Intro(Game game)
            : base(game)
        {
        }

        protected override void Init()
        {
            CanPause = false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
