using System;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    [ScriptNamespace("vtsdr")]
    internal class CarSystem : GameSystem
    {
        private RaceLevel _level;
        public GameObject CarObject;

        public override void Init(Scene level)
        {
            _level = (RaceLevel)level;

            CarObject = new GameObject();

            AnimationSequence sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c1.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c1_2.png", false), 165, 134); 
            CarObject.AnimationSequences["Forward"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/b-c1.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/b-c1_2.png", false), 165, 134);
            CarObject.AnimationSequences["b-Forward"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c3.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c3_2.png", false), 165, 134);
            CarObject.AnimationSequences["Left"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/b-c3.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/b-c3_2.png", false), 165, 134);
            CarObject.AnimationSequences["b-Left"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c13.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c13_2.png", false), 165, 134);
            CarObject.AnimationSequences["Right"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/b-c13.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/b-c13_2.png", false), 165, 134);
            CarObject.AnimationSequences["b-Right"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 100;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c1.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c5.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c6.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c7.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c8.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c9.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c10.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c11.png", false), 165, 134);
            sequence.Loop = false;
            CarObject.AnimationSequences["Crash"] = sequence;

            sequence = new AnimationSequence();
            sequence.Delay = 200;
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c11.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c10.png", false), 165, 134);
            sequence.AddSprite(_level.LoadImage("Images/Race/car/c9.png", false), 165, 134);
            sequence.Loop = false;
            CarObject.AnimationSequences["Skid"] = sequence;

            CarObject.StartAnimation("Forward");
            CarObject.AnimationCompleted += new AnimationCompletedEventHandler(AnimationCompleted);
        }

        public override void Update(CanvasContext2D context)
        {
            if (_level.Speed > 0 && _level.Status != RaceStatus.Crashing)
            {
                if (CarObject.CurrentAnimation != "Skid")
                    CarObject.Delay = Math.Floor(300 / _level.Speed);
                CarObject.Update();
            }
            else if (_level.Status == RaceStatus.Crashing)
            {
                CarObject.Update();
            }

            CarObject.Location = new Vector3D(400, 595, _level.Position + 3500);
        }

        void AnimationCompleted(object source, AnimationEndedEventArgs e)
        {
            if (e.Key == "Crash")
            {
                CarObject.StartAnimation("Forward");
                _level.Status = RaceStatus.Running;
            }
        }

        public override void Dispose()
        {
            _level = null;
        }
    }
}
