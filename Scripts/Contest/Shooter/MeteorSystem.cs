using Vtj.Gaming;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using System.Html;
using System;

namespace Vtj.Contest.Shooter
{
    internal class MeteorSystem : GameSystem
    {
        private ShooterLevel _level;
        public GameObject Ship;
        private float _speed;

        private Vector2D _gunPosition;
        private Vector2D _missileBayPosition;

        public override void Init(Scene level)
        {
            _level = (ShooterLevel)level;
            Ship = new GameObject();

            AnimationSequence sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/shooter/meteor/2.png", true), 0, 0);
            Ship.AnimationSequences["Default"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/shooter/meteor/3.png", true), 0, 0);
            Ship.AnimationSequences["Up"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/shooter/meteor/1.png", true), 0, 0);
            Ship.AnimationSequences["Down"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 40;
            sequence.Loop = false;
            sequence.AddSprites(new ImageElement[] {
                    _level.LoadImage("Images/shooter/meteor/e1.png", false),
                    _level.LoadImage("Images/shooter/meteor/e5.png", false),
                    _level.LoadImage("Images/shooter/meteor/e9.png", false),
                    _level.LoadImage("Images/shooter/meteor/e13.png", false),
                    _level.LoadImage("Images/shooter/meteor/e17.png", false)
                }, 0, 0);
            Ship.AnimationSequences["Crash"] = sequence;
            Ship.AnimationCompleted += new AnimationCompletedEventHandler(Ship_AnimationCompleted);

            _speed = 0.15f;

            Ship.Location = new Vector3D(20, 390, ShooterLevel.MeteorZ);
            Ship.StartAnimation("Default");
        }

        public override void Load()
        {
            ImageElement ship = _level.LoadImage("Images/shooter/meteor/1.png", false);

            _gunPosition = new Vector2D(ship.NaturalWidth * .8f, ship.NaturalHeight / 2);
            _missileBayPosition = new Vector2D(ship.NaturalWidth * .5f, ship.NaturalHeight * .8f);
        }

        void Ship_AnimationCompleted(object source, AnimationEndedEventArgs e)
        {
            if (e.Key == "Crash")
            {
                Ship.Visible = false;
            }
        }

        public override void Update(CanvasContext2D context)
        {
            if (_level.Status == ShooterStatus.Running)
            {
                foreach (GameObject building in _level.Buildings.VisibleBuildings)
                {
                    if (!Ship.Collides(building)) continue;
                    _level.Crash();
                    return;
                }

                foreach (Dino dino in _level.Dinos.VisibleDinos)
                {
                    if (dino.Dead || !Ship.Collides(dino)) continue;
                    _level.Crash();
                    return;
                }
            }

            Ship.Update();

            if (_level.Status != ShooterStatus.Running) return;

            float vertical = 0;
            float horizontal = 0;

            if (_level.Up)
            {
                Ship.StartAnimation("Up");
                vertical -= _speed;
            }

            if (_level.Down)
            {
                Ship.StartAnimation("Down");
                vertical += _speed;
            }

            if (!(_level.Up ^ _level.Down))
            {
                Ship.StartAnimation("Default");
            }

            if (_level.Left) horizontal -= _speed;
            if (_level.Right) horizontal += _speed;

            Ship.Location.TranslateByCoordinates(horizontal * _level.DeltaTime, vertical * _level.DeltaTime);

            if (Ship.Location.X < 0) Ship.Location.X = 0;
            else if (Ship.Location.X > 700) Ship.Location.X = 700;

            if (Ship.Location.Y < 30) Ship.Location.Y = 30;
            else if (Ship.Location.Y > 550) Ship.Location.Y = 550;
        }

        public override void Dispose()
        {
            _level = null;
            Ship = null;
        }

        public Vector2D GetAbsoluteGunPosition()
        {
            return _gunPosition.Clone().TranslateByVector(Ship.Location);
        }

        public Vector2D GetAbsoluteMissileBayPosition()
        {
            return _missileBayPosition.Clone().TranslateByVector(Ship.Location);
        }

        public void SpeedUp()
        {
            if (_speed < 0.45f) _speed += 0.1f;
        }
    }
}
