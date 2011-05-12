using Vtj.Gaming;
using System.Runtime.CompilerServices;

namespace Vtj.Contest
{
    [ScriptNamespace("vtsd")]
    internal class End : Scene
    {
        public End(Game game)
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
