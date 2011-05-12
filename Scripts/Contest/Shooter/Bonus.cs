using System;
using System.Collections.Generic;
using Vtj.Gaming;

namespace Vtj.Contest.Shooter
{
    internal class BonusTypes
    {
        public const string PowerUp = "p";
        public const string SpeedUp = "s";
        public const string Missile = "m";
        public const string DoubleShot = "2";
        public const string TripleShot = "3";
    }

    internal class Bonus : GameObject
    {
        private IMotion _motion;

        public string BonusType;

        public Bonus(Vector2D location, string bonusType)
            : base()
        {
            Location = new Vector3D(location.X, location.Y, ShooterLevel.BonusZ);

            _motion = new SineMotion(0, location.Y, 0.03f, 20);

            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprite(ShooterLevel.Current.LoadImage("images/shooter/bonus/" + bonusType + ".png", false), 0, 0);
            AnimationSequences["Default"] = sequence;
            StartAnimation("Default");

            BonusType = bonusType;
        }

        public static void PreloadImages()
        {
            ShooterLevel.Current.LoadImage("images/shooter/bonus/2.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/bonus/3.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/bonus/m.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/bonus/p.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/bonus/s.png", true);
        }

        public override void Update()
        {
            ShooterLevel level = ShooterLevel.Current;
            Location.X -= level.BaseSpeed * level.DeltaTime;

            _motion.Update(this, level.DeltaTime);

            base.Update();
        }
    }
}
