using System;
using System.Collections.Generic;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    [ScriptNamespace("vtsdr")]
    internal class CloudSystem : GameSystem
    {
        private ImageElement _explosionImage;
        private List<Explosion> _explosions;

        private List<Cloud> _clouds = new List<Cloud>();
        private ImageElement _cloudImage;
        private ImageElement _flakImage;
        private RaceLevel _level;

        private List<FlakObject> _flakObjects;

        public override void Init(Scene level)
        {
            _level = (RaceLevel)level;
            _cloudImage = _level.LoadImage("Images/Race/cloud.png", false);

            _explosionImage = _level.LoadImage("Images/Race/explosion.png", false);
            _explosions = new List<Explosion>();

            _flakImage = _level.LoadImage("Images/Race/flak.png", false);
            _flakObjects = new List<FlakObject>();

            // Build the background clouds
            for (int i = 0; i < 30; i++)
            {
                Cloud cloud = new Cloud();
                cloud.X = Math.Floor(Math.Random() * 2401) - 1200;
                cloud.Y = Math.Floor(Math.Random() * 300);
                _clouds.Add(cloud);
            }

            _clouds.Sort(delegate(Cloud x, Cloud y)
            {
                if (x.Y < y.Y) return 1;
                if (x.Y > y.Y) return -1;
                return 0;
            });
        }

        public override void Update(CanvasContext2D context)
        {
            context.Save();
            context.BeginPath();
            context.Rect(0, 0, 800, 320);
            context.ClosePath();
            context.Clip();

            float speed = -(_level.Curve * _level.Speed * 50);
            if (_level.Left && _level.Speed > 0) speed += 1;
            if (_level.Right && _level.Speed > 0) speed -= 1;

            UpdateExplosions(context, speed);

            UpdateFlak(context, speed);

            foreach (Cloud cloud in _clouds)
            {
                float scale = (400 - cloud.Y) / 400;

                cloud.X = (cloud.X - (0.3f - speed) * scale + 2400) % 2400;

                context.DrawImage(
                    _cloudImage,
                    cloud.X - 1200,
                    cloud.Y - (_cloudImage.NaturalHeight / 2) * scale,
                    _cloudImage.NaturalWidth * scale,
                    _cloudImage.NaturalHeight * scale);
            }
            context.Restore();
        }

        private void UpdateExplosions(CanvasContext2D context, float speed)
        {
            if (Math.Random() * 100 < 2)
            {
                Explosion explosion = new Explosion();
                explosion.X = Math.Random() * 800;
                explosion.start = DateTime.Now;
                _explosions.Add(explosion);
            }

            Explosion toDelete = null;

            foreach (Explosion explosion in _explosions)
            {
                explosion.X = explosion.X - (0.3f * speed);
                double y = 240 + (DateTime.Now - explosion.start) * 0.5;
                context.DrawImage(_explosionImage, explosion.X - 133, y);
                if (y >= 319) toDelete = explosion;
            }

            if (toDelete != null) _explosions.Remove(toDelete);
        }

        private void UpdateFlak(CanvasContext2D context, float speed)
        {
            if (Math.Random() * 100 < 2)
            {
                float x = Math.Random() * 800;
                float startAngle = Math.Random() * 2 - 3;
                float interval = 0.08f - Math.Random() / 100;

                for (int i = 0; i < 5 + Math.Floor(Math.Random() * 8); i++)
                {
                    _flakObjects.Add(new FlakObject(startAngle + i * interval, x, i * 400));
                }
            }

            List<FlakObject> toRemove = new List<FlakObject>();

            foreach (FlakObject flakObject in _flakObjects)
            {
                if (flakObject.Update()) toRemove.Add(flakObject);
                flakObject.X += 0.3f * speed;
                context.DrawImage(_flakImage, flakObject.X, flakObject.Y);
            }

            foreach (FlakObject flakObject in toRemove) _flakObjects.Remove(flakObject);
        }

        public override void Dispose()
        {
            _level = null;
            _clouds = null;
            _cloudImage = null;
            _explosionImage = null;
            _explosions = null;
            _flakImage = null;
            _flakObjects = null;
        }
    }
}
