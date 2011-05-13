using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using jQueryApi;
using Vtj.Gaming;

namespace Vtj.Contest.Race
{
    internal class EngineSoundSystem : GameSystem
    {
        private RaceLevel _level;
        private AudioElement _engine;
        private EngineStatus _engineStatus;

        private float EngineLowStart = 1.3f;
        private float EngineLowEnd = 5.1f;
        private float EngineHighStart = 6.2f;
        private float EngineHighEnd = 11.1f;
        private float EngineUpStart = 12.2f;
        private float EngineUpEnd = 16.9f;
        private float EngineDownStart = 18.2f;
        private float EngineDownEnd = 22.9f;

        ElementEventListener _playThroughListener;
        ElementEventListener _timeUpdatedListener;

        public override void Init(Scene level)
        {
            _level = (RaceLevel)level;
            _playThroughListener = SoundLoaded;
            _timeUpdatedListener = TimeUpdated;
            if (jQuery.Browser.Mozilla || jQuery.Browser.Opera)
                _engine = _level.LoadAudio("Audio/Race/engine.ogg");
            else
                _engine = _level.LoadAudio("Audio/Race/engine.mp3");
            _engine.AddEventListener("canplaythrough", _playThroughListener, false);
        }

        private void SoundLoaded(ElementEvent e)
        {
            _engine.RemoveEventListener("canplaythrough", _playThroughListener, false);
            _engine.AddEventListener("timeupdate", _timeUpdatedListener, false);

            _engineStatus = EngineStatus.Low;
            // _engine.CurrentTime = EngineLowStart;

            _engine.Volume = 0.5f;
            _engine.Play();
        }

        private void TimeUpdated(ElementEvent e)
        {
            if (_engineStatus == EngineStatus.Low && _engine.CurrentTime >= EngineLowEnd)
            {
                _engine.CurrentTime = EngineLowStart;
            }
            else if (_engineStatus == EngineStatus.High && _engine.CurrentTime >= EngineHighEnd)
            {
                _engine.CurrentTime = EngineHighStart;
            }
            else if (_engineStatus == EngineStatus.Down && _engine.CurrentTime >= EngineDownEnd)
            {
                _engineStatus = EngineStatus.Low;
                _engine.CurrentTime = EngineLowStart;
            }
            else if (_engineStatus == EngineStatus.Up && _engine.CurrentTime >= EngineUpEnd)
            {
                _engineStatus = EngineStatus.High;
                _engine.CurrentTime = EngineHighStart;
            }
        }

        public override void Update(CanvasContext2D context)
        {
            if (_level.Status != RaceStatus.Running && _level.Status != RaceStatus.Crashing) return;

            switch (_engineStatus)
            {
                case EngineStatus.Low:
                    if (_level.Up)
                    {
                        _engineStatus = EngineStatus.Up;
                        _engine.CurrentTime = EngineUpStart;
                    }
                    break;

                case EngineStatus.Up:
                    if (!_level.Up)
                    {
                        _engine.CurrentTime = EngineDownStart + (EngineUpEnd - _engine.CurrentTime);

                        _engineStatus = EngineStatus.Down;
                    }
                    break;

                case EngineStatus.Down:
                    if (_level.Up)
                    {
                        _engine.CurrentTime = EngineUpStart + (EngineDownEnd - _engine.CurrentTime);
                        _engineStatus = EngineStatus.Up;
                    }
                    break;

                case EngineStatus.High:
                    if (!_level.Up)
                    {
                        _engineStatus = EngineStatus.Down;
                        _engine.CurrentTime = EngineDownStart;
                    }
                    break;
            }
        }

        public override void Pause()
        {
            if (_engine == null) return;
            _engine.Pause();
        }

        public override void Resume()
        {
            if (_engine == null) return;
            _engine.Play();
        }

        public override void Dispose()
        {
            if (_engine != null)
            {
                _engine.Pause();
                _engine.RemoveEventListener("timeupdate", _timeUpdatedListener, false);
            }
            _playThroughListener = null;
            _timeUpdatedListener = null;
            _level = null;
            _engine = null;
        }
    }
}
