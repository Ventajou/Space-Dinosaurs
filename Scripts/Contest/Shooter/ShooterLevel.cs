using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using Vtj.Gaming;
using System;
using System.Collections.Generic;
using jQueryApi;

namespace Vtj.Contest.Shooter
{
    internal enum ShooterStatus
    {
        Starting = 0,
        Running = 1,
        Win = 3,
        Fail = 4
    }

    internal class ShooterLevel : Scene
    {
        #region Private Members
        private ImageElement _backgroundImage;
        private WeaponsSystem _weapons;
        private bool _practice;

        private string _startMessage;
        private string _winMessage;
        private string _failMessage;
        private int _length;
        private List<int> pendingTimers;

        private AudioElement _music;
        #endregion

        #region Public Members
        public static ShooterLevel Current;
        
        public MeteorSystem Meteor;
        public ShooterStatus Status;
        public BuildingSystem Buildings;
        public DinosSystem Dinos;
        public PlasmaSystem Plasma;
        public BonusSystem Bonus;

        public float BaseSpeed;
        public float Position;
        #endregion

        #region Public Constants
        public const int MeteorZ = 14;
        public const int WeaponsZ = 17;
        public const int DinosZ = 15;
        public const int BuildingsZ = 16;
        public const int GunZ = 18;
        public const int ExplosionZ = 11;
        public const int BonusZ = 14;
        #endregion

        public ShooterLevel(Game game, bool practice, int length)
            : base(game)
        {
            _practice = practice;
            _length = length;
            if (practice)
            {
                _startMessage = "Practice your skills by reaching and destroying the mothership.";
                _winMessage = "Congratulations! You're ready for the real game!";
                _failMessage = "Looks like you still need some more training...";
            }
            else
            {
                _startMessage = "Destroy the enemy mothership!";
                _winMessage = "<p>Mission Accomplished! Congratulations!</p>";
                _failMessage = "Oh no! You've failed your mission!...";
            }
        }

        protected override void Init()
        {
#if DEBUG
            if (Current != null) throw Exception.Create("Cannot have more than one ShooterLevel running at the same time!", null);
#endif
            pendingTimers = new List<int>();

            if (jQuery.Browser.Mozilla || jQuery.Browser.Opera)
                _music = LoadAudio("Audio/boss.ogg");
            else
                _music = LoadAudio("Audio/boss.mp3");
            
            Current = this;
            Status = ShooterStatus.Starting;
            BaseSpeed = 0.05f;
            _backgroundImage = LoadImage("Images/shooter/bg.png", false);
            AddSystem(new CloudSystem());
            AddSystem(Meteor = new MeteorSystem());
            AddSystem(Buildings = new BuildingSystem(700, _length, 3)); 
            AddSystem(Dinos = new DinosSystem(_length));
            AddSystem(_weapons = new WeaponsSystem());
            AddSystem(Plasma = new PlasmaSystem());
            AddSystem(Bonus = new BonusSystem());

            ShowMessage(_startMessage);
            Window.SetTimeout(delegate()
            {
                HideMessage();
                Status = ShooterStatus.Running;
                _music.Play();

                _music.AddEventListener("ended", delegate(ElementEvent e) { _music.Play(); }, false);
            }, 3000);
        }

        protected override void PreUpdate(CanvasContext2D context)
        {
            context.DrawImage(_backgroundImage, 0, 0);
            Position += BaseSpeed * DeltaTime;
        }

        protected override void Update(CanvasContext2D context)
        {
            if (Status == ShooterStatus.Running && Fire) _weapons.Shoot(Meteor.GetAbsoluteGunPosition(), Meteor.GetAbsoluteMissileBayPosition());
        }

        protected override void KeyDown(ElementEvent e)
        {
            switch (Status)
            {
                case ShooterStatus.Win:
                    foreach (int handle in pendingTimers) Window.ClearTimeout(handle);
                    if (_practice) CurrentGame.ShowTitleScreen();
                    else CurrentGame.NextLevel();
                    break;

                case ShooterStatus.Fail:
                    foreach (int handle in pendingTimers) Window.ClearTimeout(handle);
                    if (_practice) CurrentGame.ShowTitleScreen();
                    else CurrentGame.GameOver();
                    break;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _backgroundImage = null;
            _music.Pause();
            _music = null;
            Current = null;
        }

        protected override void PauseScene()
        {
            _music.Pause();
        }

        protected override void ResumeScene()
        {
            _music.Play();
        }

        public void Crash()
        {
            _music.Pause();
            Status = ShooterStatus.Fail;
            Meteor.Ship.StartAnimation("Crash");
            BaseSpeed = 0;
            ShowMessage(_failMessage);
            pendingTimers.Add(Window.SetTimeout(delegate()
            {
                UpdateMessage("<p>Press a key to continue.</p>");
            }, 3000));
        }

        public void Win()
        {
            _music.Pause();
            Status = ShooterStatus.Win;
            ShowMessage(_winMessage);

            pendingTimers.Add(
            Window.SetTimeout(delegate()
            {
                UpdateMessage("<p>Press a key to continue.</p>");
            }, 3000));
        }

        public void ApplyBonus(string bonusType)
        {
            switch(bonusType)
            {
                case BonusTypes.DoubleShot:
                    _weapons.ShotCount = 2;
                    break;
                case BonusTypes.Missile:
                    _weapons.AddMissile();
                    break;
                case BonusTypes.PowerUp:
                    _weapons.ShotPower = 2;
                    break;
                case BonusTypes.SpeedUp:

                    break;
                case BonusTypes.TripleShot:
                    _weapons.ShotCount = 3;
                    break;
            }
        }
    }
}

