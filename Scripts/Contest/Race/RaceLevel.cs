using System;
using System.Collections.Generic;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;
using jQueryApi;

namespace Vtj.Contest.Race
{
    internal enum EngineStatus
    {
        Up = 0,
        Down = 1,
        Low = 2,
        High = 3
    }

    internal enum RaceStatus
    {
        Starting = 0,
        Running = 1,
        Crashing = 2,
        Win = 3,
        Fail = 4
    }

    internal class RaceLevel : Scene
    {
        #region Private Members
        private const float MaxSpeed = 40;

        private ImageElement _backgroundImage;
        private ImageElement _timeLeftFrame;
        private ImageElement _meterImage;
        private ImageElement _markerImage;

        private EngineSoundSystem _engineSoundSystem;
        private NpcSystem _npcSystem;

        private int _rpm;
        private bool _practice;

        private string _startMessage;
        private string _winMessage;
        private string _failMessage;

        private List<int> pendingTimers;

        private AudioElement _music;
        #endregion

        #region Public Members
        public float Position;
        public float Shift;
        public float[] DistanceTable;
        public const int Lines = 280;
        public float Speed = 0;
        public float Curve;
        public float[] Shifts = new float[Lines];
        public float[] Curves = new float[Lines];
        public int RoadLength;
        public float TimeLeft;
        public CarSystem CarSystem;
        public RaceStatus Status;
        #endregion

        public RaceLevel(Game game, bool practice, int length)
            : base(game)
        {
            _practice = practice;
            RoadLength = length;

            if (practice)
            {
                _startMessage = "Practice your driving skills by finishing the track before the timer runs out.";
                _winMessage = "Congratulations! You're ready for the real game!";
                _failMessage = "Looks like you still need some more training...";
            }
            else
            {
                _startMessage = "Reach the research facility before it is bombed!";
                _winMessage = "<p>Congratulations! You've reached the facility in time!</p>";
                _failMessage = "Too late! You've failed to reach the facility in time...";
            }
        }

        protected override void Init()
        {
            if (jQuery.Browser.Mozilla || jQuery.Browser.Opera)
                _music = LoadAudio("Audio/race.ogg");
            else
                _music = LoadAudio("Audio/race.mp3");

            TimeLeft = RoadLength / 35;

            Position = 0;
            Shift = 0;

            pendingTimers = new List<int>();

            AddSystem(new RoadSystem());
            AddSystem(new CloudSystem());
            AddSystem(new ObstacleSystem());
            AddSystem(_npcSystem = new NpcSystem());
            AddSystem(CarSystem = new CarSystem());
            AddSystem(_engineSoundSystem = new EngineSoundSystem());

            // Get reference to game images
            _backgroundImage = LoadImage("Images/Race/bg.png", false);
            _timeLeftFrame = LoadImage("Images/Race/TimeLeft.png", false);
            _meterImage = LoadImage("Images/Race/rpm10.png", false);
            _markerImage = LoadImage("Images/Race/marker.png", false);

            // Setup the track
            Curve = 0;

            _rpm = 200;
            Status = RaceStatus.Starting;

            // Calculate the distance table for track rendering
            DistanceTable = new float[Lines];
            for (int i = 0; i < Lines + 1; i++)
            {
                DistanceTable[i] = 1000000 / (300 - i);
            }

            ShowMessage(_startMessage);
            Window.SetTimeout(delegate()
            {
                HideMessage();
                Status = RaceStatus.Running;
                _music.Play();

                _music.AddEventListener("ended", delegate(ElementEvent e) { _music.Play(); }, false);
            }, 3000);
        }

        protected override void PreUpdate(CanvasContext2D context)
        {
            // Draw background
            context.DrawImage(_backgroundImage, 0, 0);

            switch (Status)
            {
                case RaceStatus.Running:
                    // Handle left and right turns
                    if (Speed > 0)
                    {
                        float increment = (60 - Math.Max(Speed, 20)) / 80;

                        if (Left)
                        {
                            Shift += increment * DeltaTime;                            
                            CarSystem.CarObject.CurrentAnimation = Down ? "b-Left" : "Left";
                        }
                        if (Right)
                        {
                            Shift -= increment * DeltaTime;
                            CarSystem.CarObject.CurrentAnimation = Down ? "b-Right" : "Right";
                        }
                    }

                    if (!(Left ^ Right)) CarSystem.CarObject.CurrentAnimation = Down ? "b-Forward" : "Forward";

                    // Handle acceleration, braking and inertia
                    if (Down) Speed -= 0.4f;
                    else if (Up) Speed += 0.3f;
                    else Speed -= 0.1f;

                    if (Up) _rpm += 40;
                    else _rpm -= 40;
                    if (_rpm > 4500) _rpm = 4500;
                    else if (_rpm < 200) _rpm = 200;

                    // When driving off the road
                    if (Math.Abs(Shift) > 350)
                    {
                        Speed *= 0.95f;

                        if (Math.Abs(Shift) > 450)
                            Shift = (Shift / Math.Abs(Shift)) * 450;
                    }
                    break;

                case RaceStatus.Win:
                case RaceStatus.Fail:
                    Speed -= 1;
                    _rpm -= 40;
                    break;

                case RaceStatus.Crashing:
                    Speed -= 0.3f;
                    Shift -= Shift * DeltaTime / 1000;
                    break;
            }

            // Speed capping
            if (Speed > MaxSpeed) Speed = MaxSpeed;
            if (Speed < 0) Speed = 0;

            // Calculating new position
            Position += Speed * DeltaTime;

            // Drift in turns
            Shift += Curve * Speed * 150;

            if (Position >= RoadLength && Status == RaceStatus.Running)
            {
                _music.Pause();
                Status = RaceStatus.Win;
                CarSystem.CarObject.StartAnimation("Skid");
                ShowMessage(_winMessage);
                RemoveSystem(_engineSoundSystem);
                RemoveSystem(_npcSystem);

                int timeLeft = Math.Floor(TimeLeft / 1000);
                CurrentGame.Score += 1000 + timeLeft * 500;

                //if (!_practice)
                //{
                //    pendingTimers.Add(
                //    Window.SetTimeout(delegate()
                //    {
                //        UpdateMessage("<p>Time Left: " + timeLeft + "</p>");
                //    }, 1500));

                //    pendingTimers.Add(
                //    Window.SetTimeout(delegate()
                //    {
                //        UpdateMessage("<p>Score: " + CurrentGame.Score + "</p>");
                //    }, 2500));
                //}

                pendingTimers.Add(
                Window.SetTimeout(delegate()
                {
                    UpdateMessage("<p>Press a key to continue.</p>");
                }, 3000));
            }
        }

        protected override void Update(CanvasContext2D context)
        {
            if (Status != RaceStatus.Fail && Status != RaceStatus.Win)
            {
                context.DrawImage(_timeLeftFrame, 308, 10);

                Type.SetField(context, "textAlign", "right");
                context.FillStyle = "#00AD11";

                if (TimeLeft > 10000 || Math.Floor((TimeLeft / 300) % 2) != 0)
                {
                    if (TimeLeft < 0) TimeLeft = 0;
                    context.Font = "110px Digital";
                    context.FillText(Math.Floor(TimeLeft / 1000).ToString(), 475, 105);
                }

                if (Speed > 0)
                {
                    context.Save();
                    context.Scale(-1, 1);
                    long width = Math.Floor((10 * Speed) / MaxSpeed) * 22;
                    if (width > 0) context.DrawImage(_meterImage, 220 - width, 0, width, 102, -561 - width, 20, width, 102);
                    context.Restore();
                }

                context.Font = "30px Digital";
                context.FillText(Math.Floor(Speed * 5) + " Km/h", 780, 120);

                int rpmWidth = Math.Floor(_rpm / 500) * 22 + 22;

                context.DrawImage(_meterImage, 220 - rpmWidth, 0, rpmWidth, 102, 240 - rpmWidth, 20, rpmWidth, 102);
                context.FillText(Math.Floor(_rpm) + " RPM", 130, 120);

                context.BeginPath();
                context.LineWidth = 3;
                context.StrokeStyle = "#00AD11";
                context.MoveTo(5, 150);
                float x = 5 + Math.Min(Position * 735 / RoadLength, 735);
                context.LineTo(x, 150);
                context.Stroke();
                context.ClosePath();

                context.BeginPath();
                context.StrokeStyle = "#006808";
                context.MoveTo(x, 150);
                context.LineTo(790, 150);
                context.Stroke();
                context.ClosePath();
                context.DrawImage(_markerImage, x, 142);
            }

            if (Status == RaceStatus.Running)
            {
                TimeLeft -= DeltaTime;

                if (TimeLeft < 0)
                {
                    _music.Pause();
                    CarSystem.CarObject.CurrentAnimation = "Forward";
                    Status = RaceStatus.Fail;
                    ShowMessage(_failMessage);
                    RemoveSystem(_engineSoundSystem);
                    RemoveSystem(_npcSystem);
                    pendingTimers.Add(Window.SetTimeout(delegate()
                    {
                        UpdateMessage("<p>Press a key to continue.</p>");
                    }, 3000));
                }
            }
        }

        public void Crash()
        {
            if (Status == RaceStatus.Crashing) return;

            Speed = 9;
            Status = RaceStatus.Crashing;
            CarSystem.CarObject.StartAnimation("Crash");
        }

        public override void Dispose()
        {
            base.Dispose();
            _backgroundImage = null;
            _timeLeftFrame = null;
            _meterImage = null;
            _markerImage = null;
            _music.Pause();
            _music = null;
        }

        protected override void PauseScene()
        {
            _music.Pause();
        }

        protected override void ResumeScene()
        {
            _music.Play();
        }

        protected override void KeyDown(ElementEvent e)
        {
            switch (Status)
            {
                case RaceStatus.Win:
                    foreach (int handle in pendingTimers) Window.ClearTimeout(handle);
                    if (_practice) CurrentGame.ShowTitleScreen();
                    else CurrentGame.NextLevel();
                    break;

                case RaceStatus.Fail:
                    foreach (int handle in pendingTimers) Window.ClearTimeout(handle);
                    if (_practice) CurrentGame.ShowTitleScreen();
                    else CurrentGame.GameOver();
                    break;
            }
        }
    }
}
