using System.Runtime.CompilerServices;
using Vtj.Gaming;
using System.Html;
using System;
using System.Collections.Generic;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class Gun : Dino
    {
        private const int FireRate = 3500;

        private Vector2D[] _gunPositions = new Vector2D[]{
                new Vector2D(1,44),
                new Vector2D(7,26),
                new Vector2D(12,16),
                new Vector2D(30,7),
                new Vector2D(51,1),
                new Vector2D(73,6),
                new Vector2D(91,17),
                new Vector2D(96,26),
                new Vector2D(102,43)
            };


        private long _lastShot;
        private float _angle;

        public Gun(IMotion motion)
            : base(motion)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/gun/0000.png", false),
                    level.LoadImage("images/shooter/gun/0001.png", false),
                    level.LoadImage("images/shooter/gun/0002.png", false),
                    level.LoadImage("images/shooter/gun/0003.png", false),
                    level.LoadImage("images/shooter/gun/0004.png", false),
                    level.LoadImage("images/shooter/gun/0005.png", false),
                    level.LoadImage("images/shooter/gun/0006.png", false),
                    level.LoadImage("images/shooter/gun/0007.png", false),
                    level.LoadImage("images/shooter/gun/0008.png", false)
                }, 52, 73);
            sequence.Delay = 0;
            AnimationSequences[DefaultAnimation] = sequence;

            sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/gun/e0003.png", false),
                    level.LoadImage("images/shooter/gun/e0004.png", false),
                    level.LoadImage("images/shooter/gun/e0005.png", false),
                    level.LoadImage("images/shooter/gun/e0006.png", false),
                    level.LoadImage("images/shooter/gun/e0007.png", false),
                    level.LoadImage("images/shooter/gun/e0008.png", false),
                    level.LoadImage("images/shooter/gun/e0009.png", false),
                    level.LoadImage("images/shooter/gun/e0010.png", false)
                }, 60, 73);
            sequence.Delay = 50;
            sequence.Loop = false;
            AnimationSequences[ExplosionAnimation] = sequence;

            StartAnimation(DefaultAnimation);

            Life = 8;
        }

        public static void PreloadImages()
        {
            ShooterLevel level = ShooterLevel.Current;
            level.LoadImage("images/shooter/gun/0000.png", true);
            level.LoadImage("images/shooter/gun/0001.png", true);
            level.LoadImage("images/shooter/gun/0002.png", true);
            level.LoadImage("images/shooter/gun/0003.png", true);
            level.LoadImage("images/shooter/gun/0004.png", true);
            level.LoadImage("images/shooter/gun/0005.png", true);
            level.LoadImage("images/shooter/gun/0006.png", true);
            level.LoadImage("images/shooter/gun/0007.png", true);
            level.LoadImage("images/shooter/gun/0008.png", true);
            level.LoadImage("images/shooter/gun/e0003.png", false);
            level.LoadImage("images/shooter/gun/e0004.png", false);
            level.LoadImage("images/shooter/gun/e0005.png", false);
            level.LoadImage("images/shooter/gun/e0006.png", false);
            level.LoadImage("images/shooter/gun/e0007.png", false);
            level.LoadImage("images/shooter/gun/e0008.png", false);
            level.LoadImage("images/shooter/gun/e0009.png", false);
            level.LoadImage("images/shooter/gun/e0010.png", false);
        }

        public override void Update()
        {
            base.Update();

            if (Dead || !Visible) return;

            ShooterLevel level = ShooterLevel.Current;

            // Calculating the angle fire with a gross prediction on the ship's future position
            float distance = Math.Abs(Location.X - level.Meteor.Ship.Location.X) + Math.Abs(Location.Y - level.Meteor.Ship.Location.Y);
            _angle = Math.Abs(level.Meteor.Ship.Location.Clone().TranslateByCoordinates(distance / 5, 0).SetOrigin(Location).GetAbsoluteAngle());

            // And setting the frame accordingly
            CurrentFrame = 8 - Math.Floor(_angle * 9 / Math.PI);

            if (level.Ticks >= _lastShot + FireRate)
            {
                _lastShot = level.Ticks;
                new BasicPlasmaBall(Location.Clone().TranslateByVector(_gunPositions[CurrentFrame]).TranslateByCoordinates(-52, -73), 
                    new StraightMotion(new Vector2D(Math.Cos(_angle) / 10, -Math.Sin(_angle) / 10)));
            }
        }
    }
}
