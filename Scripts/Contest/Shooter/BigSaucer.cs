using System.Runtime.CompilerServices;
using Vtj.Gaming;
using System.Html;
using System;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class BigSaucer : Dino
    {
        private bool _bonusCreated = false;
        private const int FireRate = 4000;
        private long _lastShot;

        public BigSaucer(IMotion motion)
            : base(motion)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/saucer/0001.png", false),
                    level.LoadImage("images/shooter/saucer/0001.2.png", false),
                    level.LoadImage("images/shooter/saucer/0001.png", false),
                    level.LoadImage("images/shooter/saucer/0001.3.png", false)
                }, 73, 32);
            sequence.Delay = 300;
            sequence.Loop = true;
            AnimationSequences[DefaultAnimation] = sequence;

            sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/saucer/0003.png", false),
                    level.LoadImage("images/shooter/saucer/0004.png", false),
                    level.LoadImage("images/shooter/saucer/0005.png", false),
                    level.LoadImage("images/shooter/saucer/0006.png", false),
                    level.LoadImage("images/shooter/saucer/0007.png", false),
                    level.LoadImage("images/shooter/saucer/0008.png", false),
                    level.LoadImage("images/shooter/saucer/0009.png", false),
                    level.LoadImage("images/shooter/saucer/0010.png", false)
                }, 150, 62);
            sequence.Delay = 50;
            sequence.Loop = false;
            AnimationSequences[ExplosionAnimation] = sequence;

            StartAnimation(DefaultAnimation);

            Life = 16;
        }

        public static void PreloadImages()
        {
            ShooterLevel level = ShooterLevel.Current;
            level.LoadImage("images/shooter/saucer/0001.png", true);
            level.LoadImage("images/shooter/saucer/0001.2.png", true);
            level.LoadImage("images/shooter/saucer/0001.3.png", true);
            level.LoadImage("images/shooter/saucer/0003.png", false);
            level.LoadImage("images/shooter/saucer/0004.png", false);
            level.LoadImage("images/shooter/saucer/0005.png", false);
            level.LoadImage("images/shooter/saucer/0006.png", false);
            level.LoadImage("images/shooter/saucer/0007.png", false);
            level.LoadImage("images/shooter/saucer/0008.png", false);
            level.LoadImage("images/shooter/saucer/0009.png", false);
            level.LoadImage("images/shooter/saucer/0010.png", false);
        }

        public override void Update()
        {
            base.Update();

            ShooterLevel level = ShooterLevel.Current;

            if (Dead && !_bonusCreated)
            {
                level.Bonus.CreateBonus(Location);
                _bonusCreated = true;
            }
            
            if (!Dead && level.Ticks >= _lastShot + FireRate)
            {
                _lastShot = level.Ticks;

                new BasicPlasmaBall(Location, new StraightMotion(new Vector2D(-0.1f, 0)));
                new BasicPlasmaBall(Location, new StraightMotion(new Vector2D(-0.083f, -0.05f)));
                new BasicPlasmaBall(Location, new StraightMotion(new Vector2D(-0.083f, 0.05f)));
            }

        }
    }
}
