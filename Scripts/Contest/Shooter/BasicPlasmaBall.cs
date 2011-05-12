// BasicPlasmaBall.cs
//

using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html;

namespace Vtj.Contest.Shooter
{
    internal class BasicPlasmaBall : PlasmaBall
    {
        public BasicPlasmaBall(Vector2D location, IMotion motion):base(location, motion)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/plasma/1.png", false),
                    level.LoadImage("images/shooter/plasma/2.png", false),
                    level.LoadImage("images/shooter/plasma/3.png", false),
                    level.LoadImage("images/shooter/plasma/2.png", false)
                }, 6, 6);
            sequence.Loop = true;
            sequence.Delay = 200;
            AnimationSequences["Default"] = sequence;
            StartAnimation("Default");
        }

        public static void PreloadImages()
        {
            ShooterLevel.Current.LoadImage("images/shooter/plasma/1.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/plasma/2.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/plasma/3.png", true);
        }

        public override void Update()
        {
            base.Update();
            Destroyed = Hit;
        }
    }
}
