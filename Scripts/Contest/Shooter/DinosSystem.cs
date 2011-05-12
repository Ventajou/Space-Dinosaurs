using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Runtime.CompilerServices;
using System.Html.Media.Graphics;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class DinosSystem : GameSystem
    {
        private List<Dino> _dinos;
        private int _length;
        private EventHandler _killedEventHandler;
        ShooterLevel _level;

        public List<Dino> VisibleDinos = new List<Dino>();

        public DinosSystem(int length)
        {
            _length = length;
            _killedEventHandler = new EventHandler(KilledEventHandler);
        }

        public override void Init(Scene level)
        {
            _dinos = new List<Dino>();
            _level = (ShooterLevel)level;
            BigSaucer.PreloadImages();
            Pteranodon.PreloadImages();
            Gun.PreloadImages();
            Boss.PreloadImages();
            BossGun.PreloadImages();
        }

        public override void Load()
        {
            int position = 1000;

            while (position < _length)
            {
                if (_dinos.Count * (100 - position * 50 / _length) < position)
                {
                    float pick = Math.Random() * 100;

                    if (pick < 45)
                    {
                        float altitude = _level.Buildings.GunEmplacement(position - 5, position + 5);

                        Gun gun = new Gun(new StraightMotion(new Vector2D(0, 0)));
                        gun.Location = new Vector3D(position, altitude + 5, ShooterLevel.GunZ);
                        gun.KilledEvent += _killedEventHandler;
                        _dinos.Add(gun);
                    }
                    else if (pick < 80)
                    {
                        float altitude = 50 + Math.Random() * (_level.Buildings.MaxAltitude(position - 600, position) - 100);

                        for (int i = 0; i < 4; i++)
                        {
                            Pteranodon pteranodon = new Pteranodon(new SineMotion(-0.1f, altitude, 0.05f, 15));
                            pteranodon.Location = new Vector3D(position + i * 25, 0, ShooterLevel.DinosZ);
                            pteranodon.KilledEvent += _killedEventHandler;
                            _dinos.Add(pteranodon);
                        }
                    }
                    else
                    {
                        float altitude = 50 + Math.Random() * (_level.Buildings.MaxAltitude(position - 600, position) - 100);

                        BigSaucer saucer = new BigSaucer(new StraightMotion(new Vector2D(-0.05f, 0)));
                        saucer.Location = new Vector3D(position, altitude, ShooterLevel.DinosZ);
                        saucer.KilledEvent += _killedEventHandler;
                        _dinos.Add(saucer);
                    }
                }

                position += 100;
            }

            BossGun topGun = new BossGun(null);
            topGun.Location.Z--;
            _dinos.Add(topGun);

            BossGun bottomGum = new BossGun(null);
            bottomGum.StartAnimation("Gun2");
            bottomGum.Location.Z--;
            _dinos.Add(bottomGum);

            BossGun middleGun = new BossGun(null);
            middleGun.StartAnimation("Gun3");
            middleGun.Location.Z--;
            _dinos.Add(middleGun);

            Boss boss = new Boss(new TimeIndexedSineMotion(-0.05f, 300, 0.0005f, 80), topGun, middleGun, bottomGum);
            boss.Location = new Vector3D(position, 300, ShooterLevel.DinosZ);
            boss.KilledEvent += _killedEventHandler;
            _dinos.Add(boss);
        }

        void KilledEventHandler(object sender, EventArgs e)
        {
            Dino dino = (Dino)sender;

            if (VisibleDinos.Contains(dino)) VisibleDinos.Remove(dino);
            _dinos.Remove(dino);
            GameObject.Remove(dino);
        }

        public override void Update(CanvasContext2D context)
        {
            VisibleDinos = new List<Dino>();
            List<Dino> garbage = new List<Dino>();

            foreach (Dino dino in _dinos)
            {
                dino.Update();

                if (dino.Gone) garbage.Add(dino);
                else if (dino.Intersect(0, 0, 800, 600)) VisibleDinos.Add(dino);
            }

            foreach (Dino junk in garbage)
            {
                _dinos.Remove(junk);
                GameObject.Remove(junk);
            }
        }

        public override void Dispose()
        {
            _dinos = null;
            base.Dispose();
        }
    }
}
