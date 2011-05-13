using System;
using System.Collections.Generic;
using System.Debug;
using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;
using jQueryApi;

namespace Vtj.Gaming
{
    /// <summary>
    /// A game scene (level, screen...)
    /// </summary>
    internal abstract class Scene : IDisposable
    {
        #region Private Members
        private static Dictionary<string, ImageElement> _images = new Dictionary<string, ImageElement>();
        private static Dictionary<string, CanvasElement> _masks = new Dictionary<string, CanvasElement>();

        // used to keep track of elapsed time
        private DateTime _startTick;
        private DateTime _lastUpdateTick;

        // miliseconds between updates
        private int _frequency = 33;

        // The drawing canvas
        private CanvasElement _canvas;

        // List of game systems
        private List<GameSystem> _gameSystems;

        // Resources (grpahics and sounds) management
        private bool _initialized = false;
        private bool _loaded = false;
        private int _resourcesToLoad = 0;
        private int _resourcesLoaded = 0;
        private Element _resourcesLoadedLabel;
        private Element _resourcesToLoadLabel;

        private int _timerHandle = 0;

        protected Element Overlay = jQuery.Select(".GameOverlay").GetElement(0);
        private Element _message;

        private DateTime _pausedTime;
        private bool _paused = false;

        private ElementEventListener _blurListener;
        private ElementEventListener _focusListener;
        private ElementEventListener _keyDownListener;
        private ElementEventListener _keyUpListener;

        private ElementEventListener _resourceLoaded;
        private ElementEventListener _maskedResourceLoaded;
        #endregion

        #region Protected Members
        protected Game CurrentGame;

        protected bool ShowWaitMessage = true;
        protected bool CanPause = true;
        #endregion

        // Keys status
        public bool Up = false;
        public bool Down = false;
        public bool Left = false;
        public bool Right = false;
        public bool Fire = false;

        // Time passed since the scene has started
        public long Ticks;
        // Actual time passed since last sprint
        public long DeltaTime;


        public Scene(Game game)
        {
            CurrentGame = game;

            _blurListener = OnBlur;
            _focusListener = OnFocus;
            _keyDownListener = OnKeyDown;
            _keyUpListener = OnKeyUp;

            _resourceLoaded = ResourceLoaded;
            _maskedResourceLoaded = MaskedResourceLoaded;
        }

        #region Flow
        public void Start()
        {
            _initialized = false;
            _loaded = false;

            GameObject.Objects = new List<GameObject>();
            _resourcesToLoad = 0;
            _resourcesLoaded = 0;
            _gameSystems = new List<GameSystem>();

            jQuery.FromElement(Overlay).Empty();

            _resourcesLoadedLabel = Document.GetElementById("LoadedResources");
            _resourcesLoadedLabel.InnerHTML = _resourcesLoaded.ToString();
            _resourcesToLoadLabel = Document.GetElementById("TotalResources");
            _resourcesToLoadLabel.InnerHTML = _resourcesToLoad.ToString();

            if (ShowWaitMessage) jQuery.Select(".WaitMessage").Show();
            else jQuery.Select(".WaitMessage").Hide();

            // Get reference to canvas element
            _canvas = (CanvasElement)jQuery.Select(".GameCanvas").GetElement(0);

            Document.AddEventListener("blur", _blurListener, true);
            Document.AddEventListener("focus", _focusListener, true);

            Document.AddEventListener("keydown", _keyDownListener, true);
            Document.AddEventListener("keyup", _keyUpListener, true);

            Init();

            InitializeSystems();

            _initialized = true;
            HideWaitScreen();
        }

        private void TimerCallback()
        {
            if (!_initialized) return;

            DateTime now = DateTime.Now;
            Ticks = now - _startTick;
            DeltaTime = now - _lastUpdateTick;
            _lastUpdateTick = now;

            CanvasContext2D context = (CanvasContext2D)_canvas.GetContext(Rendering.Render2D);

            PreUpdate(context);

            foreach (GameSystem system in _gameSystems)
            {
                system.Update(context);
            }

            GameObject.DrawObjects(context);

            Update(context);
        }

        protected void Pause()
        {
            if (_paused || !CanPause) return;
            _paused = true;
            jQuery.Select(".PauseMessage").Show();
            _pausedTime = DateTime.Now;
            StopTimer();
            PauseScene();
            foreach (GameSystem system in _gameSystems) system.Pause();
        }

        protected virtual void PauseScene()
        {}

        protected void Resume()
        {
            if (!_paused) return;
            _paused = false;
            jQuery.Select(".PauseMessage").Hide();
            Script.Literal("{0} = {0} + {1} - {2}", _startTick, DateTime.Now, _pausedTime);
            ResumeScene();
            foreach (GameSystem system in _gameSystems) system.Resume();
            StartTimer();
        }

        protected virtual void ResumeScene()
        { }

        private void StartTimer()
        {
#if DEBUG
            if (_timerHandle != 0) Console.Log("Trying to start two timers!");
#endif
            _lastUpdateTick = _startTick = DateTime.Now;
            _timerHandle = Window.SetInterval(TimerCallback, _frequency);
            TimerCallback();

#if DEBUG
            Console.Log("Timer started [" + _timerHandle + "]");
#endif
        }

        private void StopTimer()
        {
            Window.ClearInterval(_timerHandle);
#if DEBUG
            Console.Log("Timer stopped [" + _timerHandle + "]");
#endif
            _timerHandle = 0;
        }
        #endregion

        #region Game Messages
        protected void ShowMessage(string text)
        {
            if (_message == null)
            {
                _message = Document.CreateElement("DIV");
                _message.ClassName = "GameMessage";
                Overlay.AppendChild(_message);
            }
            else jQuery.FromElement(_message).Empty();

            Element container = Document.CreateElement("DIV");
            _message.AppendChild(container);
            container.InnerHTML = text;
        }

        protected void UpdateMessage(string text)
        {
            _message.FirstChild.InnerHTML += text;
        }

        protected void HideMessage()
        {
            jQuery.FromElement(_message).Remove();
            _message = null;
        }
        #endregion

        #region Game Systems Management
        public void AddSystem(GameSystem gameSystem)
        {
            _gameSystems.Add(gameSystem);

            if (!_initialized) return;
            gameSystem.Init(this);
        }

        public void RemoveSystem(GameSystem gameSystem)
        {
            _gameSystems.Remove(gameSystem);
            gameSystem.Dispose();
        }

        private void InitializeSystems()
        {
            if (_gameSystems.Count == 0) return;
            foreach (GameSystem system in _gameSystems) system.Init(this);
        }

        private void DisposeSystems()
        {
            if (_gameSystems.Count == 0) return;
            foreach (GameSystem system in _gameSystems) system.Dispose();
            _gameSystems = null;
        }
        #endregion

        #region Resource Management
        public ImageElement LoadImage(string url, bool createMask)
        {
            if (_images.ContainsKey(url)) return _images[url];

            ImageElement image = (ImageElement)Document.CreateElement("IMG");
            _resourcesToLoad++;
            _resourcesToLoadLabel.InnerHTML = _resourcesToLoad.ToString();
            image.AddEventListener("load", createMask? _maskedResourceLoaded : _resourceLoaded, true);
            image.Src = url;
            _images[url] = image;
            return image;
        }

        public AudioElement LoadAudio(string url)
        {
            AudioElement audio = (AudioElement)Document.CreateElement("AUDIO");
            _resourcesToLoad++;
            _resourcesToLoadLabel.InnerHTML = _resourcesToLoad.ToString();
            audio.AddEventListener("canplaythrough", ResourceLoaded, false);
            audio.Src = url;
            return audio;
        }

        private void ResourceLoaded(ElementEvent e)
        {
            _resourcesLoaded++;
            _resourcesLoadedLabel.InnerHTML = _resourcesLoaded.ToString();
            HideWaitScreen();
        }

        private void MaskedResourceLoaded(ElementEvent e)
        {
            CanvasElement canvas = (CanvasElement)Document.CreateElement("CANVAS");
            ImageElement image = (ImageElement)e.Target;

            canvas.Width = image.NaturalWidth;
            canvas.Height = image.NaturalHeight;

            CanvasContext2D context = (CanvasContext2D)canvas.GetContext(Rendering.Render2D);
            context.FillStyle = "black";
            context.FillRect(0, 0, image.NaturalWidth, image.NaturalHeight);
            context.CompositeOperation = CompositeOperation.Xor;
            context.DrawImage(image, 0, 0);
            _masks[image.Src] = canvas;
            
            ResourceLoaded(e);
        }

        private void HideWaitScreen()
        {
            if (_loaded || !_initialized || _resourcesLoaded != _resourcesToLoad) return;
            _loaded = true;

            foreach (GameSystem system in _gameSystems) system.Load();

            jQuery.Select(".WaitMessage").Hide();
            StartTimer();
        }
        #endregion

        #region Event Handlers
        private void OnBlur(ElementEvent e)
        {
            if (Document.HasFocus()) return;
            Pause();
        }

        private void OnFocus(ElementEvent e)
        {
            Resume();
        }

        private void OnKeyDown(ElementEvent e)
        {
            switch (e.KeyCode)
            {
                case 37:
                    Left = true;
                    break;
                case 38:
                    Up = true;
                    break;
                case 39:
                    Right = true;
                    break;
                case 40:
                    Down = true;
                    break;
                case 32:
                    Fire = true;
                    break;
                default:
                    KeyDown(e);
                    break;
            }
        }

        private void OnKeyUp(ElementEvent e)
        {
            switch (e.KeyCode)
            {
                case 37:
                    Left = false;
                    break;
                case 38:
                    Up = false;
                    break;
                case 39:
                    Right = false;
                    break;
                case 40:
                    Down = false;
                    break;
                case 32:
                    Fire = false;
                    break;
                default:
                    KeyUp(e);
                    break;
            }
        }
        #endregion

        #region Abstract and Virtual members
        protected abstract void Init();
        protected virtual void PreUpdate(CanvasContext2D context) { }
        protected virtual void Update(CanvasContext2D context) { }
        protected virtual void KeyDown(ElementEvent e) { }
        protected virtual void KeyUp(ElementEvent e) { }
        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            _initialized = false;
            StopTimer();
            DisposeSystems();

            _resourcesLoadedLabel = null;
            _resourcesToLoadLabel = null;

            Document.RemoveEventListener("blur", _blurListener, true);
            Document.RemoveEventListener("focus", _focusListener, true);

            Document.RemoveEventListener("keydown", _keyDownListener, true);
            Document.RemoveEventListener("keyup", _keyUpListener, true);

            _canvas = null;
            _resourceLoaded = null;
            _maskedResourceLoaded = null;

            GameObject.Clear();
        }

        #endregion

        /// <summary>
        /// Performs pixel level collision between two masks
        /// </summary>
        /// <param name="mask1">The mask1.</param>
        /// <param name="mask2">The mask2.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public static bool Collision(string mask1, string mask2, float x, float y)
        {
            CanvasElement canvas1 = _masks[mask1];
            if (x > canvas1.Width || y > canvas1.Height) return false;

            CanvasElement canvas2 = _masks[mask2];
            if (canvas2.Width + x < 0 || canvas2.Height + y < 0) return false;

            int top = Math.Round(Math.Max(0, y));
            int height = Math.Round(Math.Min(canvas1.Height, y + canvas2.Height) - top);
            int left = Math.Round(Math.Max(0, x));
            int width = Math.Round(Math.Min(canvas1.Width, x + canvas2.Width) - left);

            if (width <= 0 || height <= 0) return false;

            CanvasElement checkCanvas = (CanvasElement)Document.CreateElement("Canvas");
            checkCanvas.Width = width;
            checkCanvas.Height = height;
            
            CanvasContext2D context = (CanvasContext2D)checkCanvas.GetContext(Rendering.Render2D);
            context.FillStyle = "white";
            context.FillRect(0, 0, checkCanvas.Width, checkCanvas.Height);
            context.CompositeOperation = CompositeOperation.Xor;
            
            context.DrawImage(canvas1, left, top, width, height, 0, 0, width, height);
            context.DrawImage(canvas2, Math.Round(left - x), Math.Round(top - y), width, height, 0, 0, width, height);
                        
            PixelArray data = context.GetImageData(0, 0, width, height).Data;

            for (int i = 0; i < data.Length; i += 4)
            {
                if ((int)data[i] > 0) return true;
            }

            return false;
        }
    }
}
