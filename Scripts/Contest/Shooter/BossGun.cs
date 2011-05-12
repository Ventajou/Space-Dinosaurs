// BossGun.cs
//

using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html;

namespace Vtj.Contest.Shooter
{
    internal class BossGun : Dino
    {
        private const int FireRate = 2000;
        private long _lastShot;

        public BossGun(IMotion motion)
            : base(motion)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprite(level.LoadImage("images/shooter/boss/gun1.png", false), 79, 45);
            sequence.Delay = 0;
            AnimationSequences[DefaultAnimation] = sequence;

            sequence = new AnimationSequence();
            sequence.AddSprite(level.LoadImage("images/shooter/boss/gun2.png", false), 79, 1);
            sequence.Delay = 0;
            AnimationSequences["Gun2"] = sequence;
            
            sequence = new AnimationSequence();
            sequence.AddSprite(level.LoadImage("images/shooter/boss/gun3.png", false), 100, 21);
            sequence.Delay = 0;
            AnimationSequences["Gun3"] = sequence;

            sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                level.LoadImage("images/shooter/boss/0001.png", false),
                level.LoadImage("images/shooter/boss/0002.png", false),
                level.LoadImage("images/shooter/boss/0003.png", false),
                level.LoadImage("images/shooter/boss/0004.png", false),
                level.LoadImage("images/shooter/boss/0005.png", false),
                level.LoadImage("images/shooter/boss/0006.png", false),
                level.LoadImage("images/shooter/boss/0007.png", false),
                level.LoadImage("images/shooter/boss/0008.png", false),
                level.LoadImage("images/shooter/boss/0009.png", false),
            }, 50, 50);
            sequence.Delay = 50;
            sequence.Loop = false;
            AnimationSequences[ExplosionAnimation] = sequence;

            StartAnimation(DefaultAnimation);

            Life = 150;
        }

        public static void PreloadImages()
        {
            ShooterLevel level = ShooterLevel.Current;
            level.LoadImage("images/shooter/boss/gun1.png", true);
            level.LoadImage("images/shooter/boss/gun2.png", true);
            level.LoadImage("images/shooter/boss/gun3.png", true);
            level.LoadImage("images/shooter/boss/0001.png", false);
            level.LoadImage("images/shooter/boss/0002.png", false);
            level.LoadImage("images/shooter/boss/0003.png", false);
            level.LoadImage("images/shooter/boss/0004.png", false);
            level.LoadImage("images/shooter/boss/0005.png", false);
            level.LoadImage("images/shooter/boss/0006.png", false);
            level.LoadImage("images/shooter/boss/0007.png", false);
            level.LoadImage("images/shooter/boss/0008.png", false);
            level.LoadImage("images/shooter/boss/0009.png", false);
        }

        public override void Update()
        {
            base.Update();

            ShooterLevel level = ShooterLevel.Current;

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
