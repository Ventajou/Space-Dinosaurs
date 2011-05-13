using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest
{
    internal class MenuBackgroundSystem : GameSystem
    {
        private static ImageElement _backgroundImage;

        public override void Init(Scene level)
        {
            if (_backgroundImage != null) return;
            _backgroundImage = level.LoadImage("images/bg.png", false);
        }

        public override void Update(CanvasContext2D context)
        {
            context.DrawImage(_backgroundImage, 0, 0);
        }

        public override void Dispose()
        {
        }
    }
}
