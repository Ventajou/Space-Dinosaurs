using System;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    [ScriptNamespace("vtsdr")]
    internal class ObstacleSystem : GameSystem
    {
        private RaceLevel _level;
        private RoadObject[] _objects;
        private int _objectIndex;

        public override void Init(Scene level)
        {
            _level = (RaceLevel)level;
            _objectIndex = 0;
            AddObjects(_level.RoadLength);
        }

        public override void Update(CanvasContext2D context)
        {
            while (_objectIndex < _objects.Length && _objects[_objectIndex].Distance < _level.Position) _objectIndex++;

            for (int i = _objects.Length - 1; i >= _objectIndex; i--)
            {
                RoadObject roadObject = _objects[i];
                roadObject.GameObject.Visible = false;
                float distance = roadObject.Distance - _level.Position;

                if (distance > _level.DistanceTable[RaceLevel.Lines]) continue;

                float scale = 1;
                if (distance != 0) scale = 3500 / distance;

                float y = 600 + (1000000 / distance) - 300;
                float x = 425 + roadObject.X * scale + _level.Shifts[Math.Floor(600 - y)];

                if (Number.IsNaN(x)) continue;

                roadObject.GameObject.Location = new Vector3D(x, y, roadObject.Distance);
                roadObject.GameObject.Scale = scale;
                roadObject.GameObject.Visible = true;

                if (distance < 4500 && Math.Abs(roadObject.GameObject.Location.X - _level.CarSystem.CarObject.Location.X) < 85)
                {
                    _level.Crash();
                }
            }
        }

        private void AddObjects(long length)
        {
            _objects = new RoadObject[0];
            long distance = 10000;
            while (distance < length + 50000)
            {
                if (Math.Random() * 11 > 4)
                {
                    _objects[_objects.Length] = PickObject(distance, -480);
                }

                if (Math.Random() * 11 > 4)
                {
                    _objects[_objects.Length] = PickObject(distance, 480);
                }

                distance += 10000;
            }

            _objects[_objects.Length] = new Sign(length + 35000, 480, _level);
        }

        private RoadObject PickObject(long distance, float x)
        {
            float rand = Math.Random() * 100;

            if (rand < 70)
            {
                float shift = Math.Random() * 700 * Math.Abs(x) / x;
                return new Boulder(distance, x + shift, _level);
            }
            else if (rand < 90)
            {
                return new Pole(distance, x, _level);
            }
            else
            {
                return new Tree(distance, x, _level);
            }
        }

        public override void Dispose()
        {
            _level = null;
            _objects = null;
        }
    }
}
