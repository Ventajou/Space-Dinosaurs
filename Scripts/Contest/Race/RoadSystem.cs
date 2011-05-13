using System;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    internal class RoadSystem : GameSystem
    {
        private RaceLevel _level;
        private RoadEvent[] _events;
        private ImageElement[] _road = new ImageElement[2];

        public override void Init(Scene level)
        {
            _level = (RaceLevel)level;

            _road[0] = _level.LoadImage("Images/Race/road1.png", false);
            _road[1] = _level.LoadImage("Images/Race/road2.png", false);

            BuildEvents(_level.RoadLength);
        }

        public override void Update(CanvasContext2D context)
        {
            RoadEvent roadEvent = null;
            int trackIndex = 0;
            while (trackIndex < _events.Length && _events[trackIndex].Distance <= _level.Position)
            {
                roadEvent = _events[trackIndex];
                trackIndex++;
            }

            float shift = 0;
            float curve = 0;

            for (int i = 0; i < RaceLevel.Lines; i++)
            {
                float distance = _level.Position + _level.DistanceTable[i];
                while ((trackIndex < _events.Length) && (_events[trackIndex].Distance <= distance))
                {
                    roadEvent = _events[trackIndex];
                    trackIndex++;
                }

                if (trackIndex < _events.Length)
                {
                    RoadEvent nextEvent = _events[trackIndex];

                    curve = (nextEvent.Curve - roadEvent.Curve) * (distance - roadEvent.Distance) / (nextEvent.Distance - roadEvent.Distance) + roadEvent.Curve;
                }

                long index = Math.Round(distance / 2000) % 2;

                if (i > 0) shift += curve * (_level.DistanceTable[i + 1] - _level.DistanceTable[i]);

                _level.Curves[i] = curve;
                _level.Shifts[i] = Math.Round(((299 - i) * _level.Shift) / RaceLevel.Lines) - 25 + shift;

                context.DrawImage(_road[index], 0, 299 - i, 850, 1, _level.Shifts[i], 599 - i, 850, 1);

                if (i == 0) _level.Curve = curve;
            }
        }

        private void BuildEvents(long length)
        {
            _events = new RoadEvent[0];

            RoadEvent firstEvent = new RoadEvent();
            firstEvent.Distance = 0;
            firstEvent.Curve = 0;
            _events[_events.Length] = firstEvent;

            long distance = 30000 + Math.Floor(Math.Random() * 100000);
            while (distance < length - 10000)
            {
                RoadEvent roadEvent = new RoadEvent();

                roadEvent.Distance = distance;
                roadEvent.Curve = 0.005f - Math.Random() * 0.01f;

                if (Math.Random() * 11 > 4) roadEvent.Curve = 0;

                _events[_events.Length] = roadEvent;

                distance += 10000 + Math.Floor(Math.Random() * 100000);
            }

            RoadEvent lastEvent = new RoadEvent();
            lastEvent.Distance = length;
            lastEvent.Curve = 0;
            _events[_events.Length] = lastEvent;
        }

        public override void Dispose()
        {
            _level = null;
            _road = null;
            _events = null;
        }
    }
}
