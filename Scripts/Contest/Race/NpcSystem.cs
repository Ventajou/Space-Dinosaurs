using System;
using System.Collections;
using System.Collections.Generic;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    [ScriptNamespace("vtsdr")]
    internal class NpcSystem : GameSystem
    {
        RaceLevel _level;
        public List<Npc> _npcs;

        private Dictionary<string, AnimationSequence> _animationSequences;

        public override void Init(Scene level)
        {
            _level = (RaceLevel)level;
            EnsureAnimationSequences();
            AddNpcs(_level.RoadLength);
        }

        public override void Update(CanvasContext2D context)
        {
            foreach(Npc npc in _npcs)
            {
                npc.Object.Visible = false;
                npc.Distance += npc.Speed * _level.DeltaTime;

                float distance = npc.Distance - _level.Position;

                if (distance <= 0 || distance > _level.DistanceTable[RaceLevel.Lines]) continue;

                float scale = 3500 / distance;

                float y = 600 + (1000000 / distance) - 300;
                float x = 425 + npc.X * scale + _level.Shifts[Math.Floor(600 - y)];

                if (Number.IsNaN(x)) continue;

                int index = 3;
                if (distance < 30000)
                {
                    int angle = Math.Floor(_level.Shifts[Math.Floor(600 - y)] + (npc.X * 3500) / distance);
                    index = Math.Floor(Math.Abs(angle) / 60);
                    if (index > 3) index = 3;
                    index = 3 + index * (angle / Math.Abs(angle));
                    if (Number.IsNaN(index)) index = 3;
                }                

                npc.Object.Update();
                npc.Object.Visible = true;
                npc.Object.Location = new Vector3D(x, y, npc.Distance);
                npc.Object.Scale = scale;
                npc.Object.CurrentFrame = index;

                if (distance < 4500 && Math.Abs(npc.Object.Location.X - _level.CarSystem.CarObject.Location.X) < 100)
                {
                    _level.Crash();
                }
            }
        }

        private void AddNpcs(long length)
        {
            _npcs = new List<Npc>();
            long distance = 50000;
            while (distance < length / 2)
            {
                if (Math.Random() * 11 > 4)
                {
                    Npc npc = new Npc();
                    npc.Distance = distance;
                    npc.Speed = 10 + Math.Random() * 11;
                    npc.X = (Math.Floor(Math.Random() * 3) * 260) - 260;
                    npc.Object = PickNpcObject();
                    npc.Object.Delay = Math.Round(300 / npc.Speed);
                    _npcs.Add(npc);
                }

                distance += 30000;
            }
        }

        private GameObject PickNpcObject()
        {
            GameObject car = new GameObject();

            if (Math.Floor(Math.Random() * 2) == 0)
                car.AnimationSequences["Default"] = _animationSequences["Npc1"];
            else
                car.AnimationSequences["Default"] = _animationSequences["Npc2"];

            car.SetSprite("Default", 2);
            return car;
        }

        private void EnsureAnimationSequences()
        {
            if (_animationSequences != null) return;

            _animationSequences = new Dictionary();

            AnimationSequence sequence = new AnimationSequence();
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/1.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/2.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/3.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/4.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/5.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/6.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc1/7.png", false), 252 / 2, 116);
            _animationSequences["Npc1"] = sequence;

            sequence = new AnimationSequence();
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/1.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/2.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/3.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/4.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/5.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/6.png", false), 252 / 2, 116);
            sequence.AddSprite(_level.LoadImage("Images/Race/npc2/7.png", false), 252 / 2, 116);
            _animationSequences["Npc2"] = sequence;
        }

        public override void Dispose()
        {
            _animationSequences = null;
            _level = null;
            _npcs = null;
        }
    }
}
