// boss.cs
//

using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html;

namespace Vtj.Contest.Shooter
{
    internal class Boss : Dino
    {
        private const int FireRate = 1000;
        private long _lastShot;

        private bool _dead;

        private BossGun _topGun;
        private BossGun _middleGun;
        private BossGun _bottomGun;

        List<GameObject> _explosions;

        public Boss(IMotion motion, BossGun topGun, BossGun middleGun, BossGun bottomGun)
            : base(motion)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/boss/boss.png", false)
                }, 0, 133);
            sequence.Delay = 200;
            sequence.Loop = true;
            AnimationSequences[DefaultAnimation] = sequence;

            _topGun = topGun;
            _middleGun = middleGun;
            _bottomGun = bottomGun;

            sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/boss/boss.png", false)
                }, 0, 133);
            sequence.Delay = 500;
            sequence.Loop = true;
            AnimationSequences[ExplosionAnimation] = sequence;

            StartAnimation(DefaultAnimation);

            Life = 300;
        }

        public static void PreloadImages()
        {
            ShooterLevel level = ShooterLevel.Current;
            level.LoadImage("images/shooter/boss/boss.png", true);
        }

        public override void Update()
        {
            base.Update();

            if (_explosions != null) foreach (GameObject gameObject in _explosions) gameObject.Update();

            if (Dead && !_dead)
            {
                _dead = true;
                TimeIndexedSineMotion motion = (TimeIndexedSineMotion)Motion;
                motion.Amplitude = 0;
                _explosions = new List<GameObject>();
                for (int i = 0; i < 10; i++)
                {
                    Explosion explosion = new Explosion();
                    explosion.Location = new Vector3D(Location.X + Math.Random() * 300, Location.Y - 133 + Math.Random() * 266, 10);
                    _explosions.Add(explosion);
                }

                ShooterLevel.Current.Win();
            }

            if (Dead) return;

            if (Location.X <= 500)
            {
                TimeIndexedSineMotion motion = (TimeIndexedSineMotion)Motion;
                motion.Speed = ShooterLevel.Current.BaseSpeed;
            }

            if (_topGun != null)
            {
                _topGun.Location.X = Location.X + 220;
                _topGun.Location.Y = Location.Y - 132;

                if (_topGun.Dead) _topGun = null;
            }

            if (_bottomGun != null)
            {
                _bottomGun.Location.X = Location.X + 220;
                _bottomGun.Location.Y = Location.Y + 132;

                if (_bottomGun.Dead) _bottomGun = null;
            }

            if (_middleGun != null)
            {
                _middleGun.Location.X = Location.X + 4;
                _middleGun.Location.Y = Location.Y;

                if (_middleGun.Dead) _middleGun = null;
            }

            if (_topGun == null && _middleGun == null && _bottomGun == null)
            {
                ShooterLevel level = ShooterLevel.Current;
                if (!Dead && level.Ticks >= _lastShot + FireRate)
                {
                    _lastShot = level.Ticks;

                    new BasicPlasmaBall(Location, new StraightMotion(new Vector2D(Math.Random() * -0.1f, 0.05f - Math.Random() * 0.1f)));
                    new BasicPlasmaBall(Location, new StraightMotion(new Vector2D(Math.Random() * -0.1f, 0.05f - Math.Random() * 0.1f)));
                    new BasicPlasmaBall(Location, new StraightMotion(new Vector2D(Math.Random() * -0.1f, 0.05f - Math.Random() * 0.1f)));
                }
            }
            else Life = 300;
        }
    }
}
