using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html;

namespace Vtj.Contest.Shooter
{
    internal class Pteranodon : Dino
    {
        public Pteranodon(IMotion motion)
            : base(motion)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/ptera/0001.png", false),
                    level.LoadImage("images/shooter/ptera/0002.5.png", false),
                    level.LoadImage("images/shooter/ptera/0002.png", false),
                    level.LoadImage("images/shooter/ptera/0002.5.png", false),
                    level.LoadImage("images/shooter/ptera/0001.png", false),
                    level.LoadImage("images/shooter/ptera/0003.5.png", false),
                    level.LoadImage("images/shooter/ptera/0003.png", false),
                    level.LoadImage("images/shooter/ptera/0003.5.png", false)
                }, 0, 0);
            sequence.Delay = 200;
            sequence.Loop = true;
            AnimationSequences[DefaultAnimation] = sequence;

            sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/ptera/0005.png", false),
                    level.LoadImage("images/shooter/ptera/0006.png", false),
                    level.LoadImage("images/shooter/ptera/0007.png", false),
                    level.LoadImage("images/shooter/ptera/0008.png", false),
                    level.LoadImage("images/shooter/ptera/0009.png", false),
                    level.LoadImage("images/shooter/ptera/0010.png", false),
                    level.LoadImage("images/shooter/ptera/0011.png", false),
                    level.LoadImage("images/shooter/ptera/0012.png", false),
                    level.LoadImage("images/shooter/ptera/0013.png", false),
                    level.LoadImage("images/shooter/ptera/0014.png", false),
                    level.LoadImage("images/shooter/ptera/0015.png", false)
                }, 0, 0);
            sequence.Delay = 50;
            sequence.Loop = false;
            AnimationSequences[ExplosionAnimation] = sequence;

            StartAnimation(DefaultAnimation);

            Life = 4;
        }

        public static void PreloadImages()
        {
            ShooterLevel level = ShooterLevel.Current;
            level.LoadImage("images/shooter/ptera/0003.5.png", true);
            level.LoadImage("images/shooter/ptera/0002.5.png", true);
            level.LoadImage("images/shooter/ptera/0001.png", true);
            level.LoadImage("images/shooter/ptera/0002.png", true);
            level.LoadImage("images/shooter/ptera/0003.png", true);
            level.LoadImage("images/shooter/ptera/0005.png", false);
            level.LoadImage("images/shooter/ptera/0006.png", false);
            level.LoadImage("images/shooter/ptera/0007.png", false);
            level.LoadImage("images/shooter/ptera/0008.png", false);
            level.LoadImage("images/shooter/ptera/0009.png", false);
            level.LoadImage("images/shooter/ptera/0010.png", false);
            level.LoadImage("images/shooter/ptera/0011.png", false);
            level.LoadImage("images/shooter/ptera/0012.png", false);
            level.LoadImage("images/shooter/ptera/0013.png", false);
            level.LoadImage("images/shooter/ptera/0014.png", false);
            level.LoadImage("images/shooter/ptera/0015.png", false);
        }
    }
}
