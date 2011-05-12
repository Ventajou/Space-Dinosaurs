using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Html;

namespace Vtj.Contest.Shooter
{
    internal class Missile : Projectile
    {
        public static long LastFired;
        public const int FireRate = 1000;

        protected override int Strength
        {
            get { return 8; }
        }

        public Missile(Vector2D location)
            : base(location, new SeekMotion(null, 12, new Vector2D(12, 0), 0.25f))
        {
            WeaponsSystem.TotalMissiles++;

            LoadAnimation("00", "8");
            LoadAnimation("01", "9");
            LoadAnimation("02", "10");
            LoadAnimation("03", "11");
            LoadAnimation("04", "12");
            LoadAnimation("05", "13");
            LoadAnimation("06", "14");
            LoadAnimation("07", "15");
            LoadAnimation("08", "0");
            LoadAnimation("09", "1");
            LoadAnimation("10", "2");
            LoadAnimation("11", "3");
            LoadAnimation("12", "4");
            LoadAnimation("13", "5");
            LoadAnimation("14", "6");
            LoadAnimation("15", "7");

            StartAnimation("0");

            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/explosion/0002.png", false),
                    level.LoadImage("images/shooter/explosion/0003.png", false),
                    level.LoadImage("images/shooter/explosion/0004.png", false),
                    level.LoadImage("images/shooter/explosion/0005.png", false),
                    level.LoadImage("images/shooter/explosion/0006.png", false),
                    level.LoadImage("images/shooter/explosion/0007.png", false),
                    level.LoadImage("images/shooter/explosion/0008.png", false),
                    level.LoadImage("images/shooter/explosion/0009.png", false)
                }, 25, 25);
            sequence.Delay = 50;
            sequence.Loop = false;
            AnimationSequences["Explosion"] = sequence;

            Dino dino = AcquireTarget();
            if (dino != null) dino.Locked = true;
            ((SeekMotion)Motion).Target = AcquireTarget();
        }

        public override void Update()
        {
            base.Update();
            if (Hit && CurrentAnimation != "Explosion")
            {
                StartAnimation("Explosion");
                Location.Z = ShooterLevel.ExplosionZ;
                Motion = new StraightMotion(new Vector2D(0, 0));
            }

            Destroyed = Hit && !Started;

            if (Hit) return;

            SeekMotion motion = (SeekMotion)Motion;

            if (Hit && motion.Target != null) ((Dino)motion.Target).Locked = false;

            CurrentAnimation = Math.Abs((Math.Round(motion.Angle / 0.4188f) - 16) % 16).ToString();

            // Missiles do not pick a new target
            if (motion.Target != null && ((Dino)motion.Target).Dead) motion.Target = null;
        }

        private void LoadAnimation(string number, string name)
        {
            ShooterLevel level = ShooterLevel.Current;
            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprites(new ImageElement[]{
                    level.LoadImage("images/shooter/missile/" + number + "00.png", false),
                    level.LoadImage("images/shooter/missile/" + number + "01.png", false),
                    level.LoadImage("images/shooter/missile/" + number + "02.png", false)
                }, 30, 30);
            AnimationSequences[name] = sequence;
        }

        public static void PreloadImages()
        {
            ShooterLevel level = ShooterLevel.Current;

            level.LoadImage("images/shooter/missile/0000.png", true);
            level.LoadImage("images/shooter/missile/0001.png", true);
            level.LoadImage("images/shooter/missile/0002.png", true);
            level.LoadImage("images/shooter/missile/0100.png", true);
            level.LoadImage("images/shooter/missile/0101.png", true);
            level.LoadImage("images/shooter/missile/0102.png", true);
            level.LoadImage("images/shooter/missile/0200.png", true);
            level.LoadImage("images/shooter/missile/0201.png", true);
            level.LoadImage("images/shooter/missile/0202.png", true);
            level.LoadImage("images/shooter/missile/0300.png", true);
            level.LoadImage("images/shooter/missile/0301.png", true);
            level.LoadImage("images/shooter/missile/0302.png", true);
            level.LoadImage("images/shooter/missile/0400.png", true);
            level.LoadImage("images/shooter/missile/0401.png", true);
            level.LoadImage("images/shooter/missile/0402.png", true);
            level.LoadImage("images/shooter/missile/0500.png", true);
            level.LoadImage("images/shooter/missile/0501.png", true);
            level.LoadImage("images/shooter/missile/0502.png", true);
            level.LoadImage("images/shooter/missile/0600.png", true);
            level.LoadImage("images/shooter/missile/0601.png", true);
            level.LoadImage("images/shooter/missile/0602.png", true);
            level.LoadImage("images/shooter/missile/0700.png", true);
            level.LoadImage("images/shooter/missile/0701.png", true);
            level.LoadImage("images/shooter/missile/0702.png", true);
            level.LoadImage("images/shooter/missile/0800.png", true);
            level.LoadImage("images/shooter/missile/0801.png", true);
            level.LoadImage("images/shooter/missile/0802.png", true);
            level.LoadImage("images/shooter/missile/0900.png", true);
            level.LoadImage("images/shooter/missile/0901.png", true);
            level.LoadImage("images/shooter/missile/0902.png", true);
            level.LoadImage("images/shooter/missile/1000.png", true);
            level.LoadImage("images/shooter/missile/1001.png", true);
            level.LoadImage("images/shooter/missile/1002.png", true);
            level.LoadImage("images/shooter/missile/1100.png", true);
            level.LoadImage("images/shooter/missile/1101.png", true);
            level.LoadImage("images/shooter/missile/1102.png", true);
            level.LoadImage("images/shooter/missile/1200.png", true);
            level.LoadImage("images/shooter/missile/1201.png", true);
            level.LoadImage("images/shooter/missile/1202.png", true);
            level.LoadImage("images/shooter/missile/1300.png", true);
            level.LoadImage("images/shooter/missile/1301.png", true);
            level.LoadImage("images/shooter/missile/1302.png", true);
            level.LoadImage("images/shooter/missile/1400.png", true);
            level.LoadImage("images/shooter/missile/1401.png", true);
            level.LoadImage("images/shooter/missile/1402.png", true);
            level.LoadImage("images/shooter/missile/1500.png", true);
            level.LoadImage("images/shooter/missile/1501.png", true);
            level.LoadImage("images/shooter/missile/1502.png", true);

            level.LoadImage("images/shooter/explosion/0002.png", false);
            level.LoadImage("images/shooter/explosion/0003.png", false);
            level.LoadImage("images/shooter/explosion/0004.png", false);
            level.LoadImage("images/shooter/explosion/0005.png", false);
            level.LoadImage("images/shooter/explosion/0006.png", false);
            level.LoadImage("images/shooter/explosion/0007.png", false);
            level.LoadImage("images/shooter/explosion/0008.png", false);
            level.LoadImage("images/shooter/explosion/0009.png", false);
        }

        private Dino AcquireTarget()
        {
            List<Dino> dinos = ShooterLevel.Current.Dinos.VisibleDinos;

            if (dinos.Count == 0) return null;

            foreach (Dino dino in dinos)
            {
                if (!dino.Locked) return dino;
            }

            return dinos[dinos.Count - 1];
        }

        public override void Removed()
        {
            WeaponsSystem.TotalMissiles--;
        }
    }
}
