using System.Html;
using System.Runtime.CompilerServices;
using jQueryApi;
using Vtj.Gaming;

namespace Vtj.Contest
{
    [ScriptNamespace("vtsd")]
    internal class GameOver : Scene
    {
        Element _overlay = jQuery.Select(".GameOverlay").GetElement(0);

        public GameOver(Game game)
            : base(game)
        {
            ShowWaitMessage = false;
        }

        protected override void Init()
        {
            AddSystem(new MenuBackgroundSystem());

            CanPause = false;

            BuildMenuButton("Back", 530, delegate(ElementEvent e)
            {
                CurrentGame.ShowTitleScreen();
            });

            Element title = BuildMenuButton("Game Over", 80, null);
            title.ClassName += " MenuTitle";

            Element wrapper = Document.CreateElement("DIV");
            _overlay.AppendChild(wrapper);
            wrapper.ClassName = "MenuText";

            wrapper.InnerHTML = "<p>You have failed your mission and the Space Dinosaurs have invaded the Earth...</p>";
        }

        private Element BuildMenuButton(string label, int position, ElementEventListener clickListener)
        {
            Element wrapper = Document.CreateElement("DIV");
            _overlay.AppendChild(wrapper);
            wrapper.ClassName = "MenuButton";
            wrapper.Style.Top = position + "px";
            Element button = Document.CreateElement("SPAN");
            wrapper.AppendChild(button);
            button.InnerHTML = label;
            if (clickListener != null) button.AddEventListener("click", clickListener, false);
            return wrapper;
        }
    }
}
