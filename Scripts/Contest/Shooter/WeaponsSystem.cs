using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Runtime.CompilerServices;
using System.Html.Media.Graphics;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class WeaponsSystem : GameSystem
    {
        public static int TotalMissiles;

        private List<Projectile> _projectiles;
        private ShooterLevel _level;
        private int _lastShotIndex;

        public int ShotCount;
        public int MissileCount;
        public int ShotPower;

        public override void Init(Scene level)
        {
            TotalMissiles = 0;
            ShotCount = 1;
            MissileCount = 0;
            ShotPower = 1;

            Bullet.PreloadImages();
            Missile.PreloadImages();
            Bullet.LastFired = 0;
            Missile.LastFired = 0;

            _projectiles = new List<Projectile>();
            _level = (ShooterLevel)level;
        }

        public override void Update(CanvasContext2D context)
        {
            List<Projectile> garbage = new List<Projectile>();
            foreach (Projectile projectile in _projectiles)
            {
                projectile.Update();
                if (!projectile.Intersect(0, 0, 800, 600) || projectile.Destroyed) garbage.Add(projectile);
            }

            foreach (Projectile junk in garbage)
            {
                junk.Removed();
                GameObject.Remove(junk);
                _projectiles.Remove(junk);
            }
        }

        public void AddMissile()
        {
            if (MissileCount < 4) MissileCount++;
        }

        public void Shoot(Vector2D gunLocation, Vector2D missileBayLocation)
        {
            switch (ShotCount)
            {
                case 3:
                    if (_level.Ticks >= Bullet.LastFired + Bullet.FireRate)
                    {
                        Bullet.LastFired = _level.Ticks;
                        _projectiles.Add(new Bullet(gunLocation, ShotPower, new StraightMotion(new Vector2D(0.41f, 0.25f))));
                        _projectiles.Add(new Bullet(gunLocation, ShotPower, new StraightMotion(new Vector2D(0.5f, 0))));
                        _projectiles.Add(new Bullet(gunLocation, ShotPower, new StraightMotion(new Vector2D(0.41f, -0.25f))));
                    }
                    break;
                case 2:
                    if (_level.Ticks >= Bullet.LastFired + Bullet.FireRate / 2)
                    {
                        _lastShotIndex = (_lastShotIndex + 1) % 2;
                        Bullet.LastFired = _level.Ticks;
                        _projectiles.Add(new Bullet(gunLocation.Clone().TranslateByCoordinates(0, _lastShotIndex * -5), ShotPower, new StraightMotion(new Vector2D(0.5f, 0))));
                    }
                    break;
                default:
                    if (_level.Ticks >= Bullet.LastFired + Bullet.FireRate)
                    {
                        Bullet.LastFired = _level.Ticks;
                        _projectiles.Add(new Bullet(gunLocation, ShotPower, new StraightMotion(new Vector2D(0.5f, 0))));
                    }
                    break;
            }

            if (TotalMissiles < MissileCount && _level.Ticks >= Missile.LastFired + Missile.FireRate)
            {
                Missile.LastFired = _level.Ticks;
                _projectiles.Add(new Missile(missileBayLocation));
            }
        }

        public override void Resume()
        {
            Bullet.LastFired = 0;
            Missile.LastFired = 0;
        }
    }
}
