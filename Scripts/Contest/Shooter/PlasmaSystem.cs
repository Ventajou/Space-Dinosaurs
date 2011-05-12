using System;
using System.Collections.Generic;
using Vtj.Gaming;
using System.Runtime.CompilerServices;
using System.Html.Media.Graphics;

namespace Vtj.Contest.Shooter
{
    [ScriptNamespace("vtsds")]
    internal class PlasmaSystem : GameSystem
    {
        List<PlasmaBall> _plasmaBalls;
        ShooterLevel _level;

        public override void Init(Scene level)
        {
            _plasmaBalls = new List<PlasmaBall>();
            _level = (ShooterLevel)level;

            BasicPlasmaBall.PreloadImages();
        }

        public override void Update(CanvasContext2D context)
        {
            List<PlasmaBall> garbage = new List<PlasmaBall>();
            foreach (PlasmaBall plasmaBall in _plasmaBalls)
            {
                plasmaBall.Update();
                if (!plasmaBall.Intersect(0, 0, 900, 600) || plasmaBall.Destroyed) garbage.Add(plasmaBall);
            }

            foreach (PlasmaBall junk in garbage)
            {
                GameObject.Remove(junk);
                _plasmaBalls.Remove(junk);
            }
        }

        public void AddPlasmaBall(PlasmaBall ball)
        {
            _plasmaBalls.Add(ball);
        }
    }
}
