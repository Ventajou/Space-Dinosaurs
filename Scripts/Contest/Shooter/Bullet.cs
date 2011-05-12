using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Runtime.CompilerServices;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class Bullet : Projectile
    {
        public static long LastFired;
        public const int FireRate = 200;
        int _strength;

        protected override int Strength { get { return _strength; } }

        public Bullet(Vector2D location, int strength, IMotion motion)
            : base(location, motion)
        {
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprite(ShooterLevel.Current.LoadImage((strength == 1)? "images/shooter/meteor/sb.png" : "images/shooter/meteor/b.png", false), 0, 0);
            AnimationSequences["Default"] = sequence;
            StartAnimation("Default");
            _strength = strength;
        }

        public static void PreloadImages()
        {
            ShooterLevel.Current.LoadImage("images/shooter/meteor/b.png", true);
            ShooterLevel.Current.LoadImage("images/shooter/meteor/sb.png", true);
        }

        public override void Update()
        {
            base.Update();
            Destroyed = Hit;
        }
    }
}
