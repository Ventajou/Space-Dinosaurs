using System;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using jQueryApi;
using Vtj.Contest.Race;
using Vtj.Contest.Shooter;
using Vtj.Gaming;

namespace Vtj.Contest
{
    internal class Title : Scene
    {
        ImageElement _titleImage;
        Element _overlay;

        public Title(Game game)
            : base(game)
        {
            ShowWaitMessage = false;
        }

        protected override void Init()
        {
            AddSystem(new MenuBackgroundSystem());

            CanPause = false;
            _overlay = jQuery.Select(".GameOverlay").GetElement(0);
            _titleImage = base.LoadImage("images/title/title.png", false);
            jQuery.FromElement(_overlay).Show();
            BuildMainMenu();
        }

        protected override void Update(CanvasContext2D context)
        {
            context.DrawImage(_titleImage, (800 - _titleImage.NaturalWidth) / 2, 90 + Math.Cos(Ticks / 1000) * 20);
        }

        public override void Dispose()
        {
            base.Dispose();

            jQuery.FromElement(_overlay).Empty();
            _titleImage = null;
            _overlay = null;
        }

        private void BuildMainMenu()
        {
            jQuery.FromElement(_overlay).Empty();

            BuildMenuButton("Play!", 370, delegate(ElementEvent e)
                {
                    CurrentGame.Play();
                });

            BuildMenuButton("Practice", 420, delegate(ElementEvent e)
                {
                    BuildPracticeMenu();
                });

            //BuildMenuButton("Options", 470, delegate(ElementEvent e)
            //    {
            //        BuildOptionsMenu();
            //    });

            //BuildMenuButton("High Scores", 520, delegate(ElementEvent e)
            //    {
            //        CurrentGame.ShowHiScores();
            //    });

            jQuery.FromElement(_overlay).Show();
        }

        private void BuildPracticeMenu()
        {
            jQuery.FromElement(_overlay).Empty();

            BuildMenuButton("Race", 395, delegate(ElementEvent e)
            {
                CurrentGame.LoadScene(new RaceLevel(CurrentGame, true, 3000000));
            });

            BuildMenuButton("Shooter", 445, delegate(ElementEvent e)
            {
                CurrentGame.LoadScene(new ShooterLevel(CurrentGame, true, 10000));
            });

            BuildMenuButton("Back", 495, delegate(ElementEvent e)
            {
                BuildMainMenu();
            });
        }

        private void BuildOptionsMenu()
        {
            jQuery.FromElement(_overlay).Empty();

            BuildMenuButton("Sound", 395, delegate(ElementEvent e)
            {
                Script.Alert("Not Implemented Yet!");
            });

            BuildMenuButton("Controls", 445, delegate(ElementEvent e)
            {
                Script.Alert("Not Implemented Yet!");
            });

            BuildMenuButton("Back", 495, delegate(ElementEvent e)
            {
                BuildMainMenu();
            });
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
            button.AddEventListener("click", clickListener, false);
            return wrapper;
        }
    }
}
