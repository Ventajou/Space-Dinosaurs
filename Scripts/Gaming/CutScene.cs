using System.Html;
using System.Html.Media.Graphics;
using System.Runtime.CompilerServices;

namespace Vtj.Gaming
{
    [ScriptNamespace("vtg")]
    internal class CutScene : Scene
    {
        private string _imagePath;
        private string[] _text;
        private Element _textElement;
        private bool _keyPressed;
        private int _paragraphIndex;
        private int _charIndex;
        private bool _finished;
        private Game _game;
        private long _nextLetter;

        public CutScene(Game game, string imagePath, string text)
            : base(game)
        {
            _imagePath = imagePath;
            _text = text.Split('\n');
            _game = game;

            CanPause=false;
            ShowWaitMessage=false;
        }

        protected override void Init()
        {
            Element div = Document.CreateElement("DIV");
            div.ClassName = "CutScene";
            Overlay.AppendChild(div);

            div.AppendChild(LoadImage(_imagePath, false));
            _textElement = Document.CreateElement("DIV");
            div.AppendChild(_textElement);

            _paragraphIndex = 0;
            _charIndex = 0;
            _finished = false;
            _nextLetter = 0;
        }

        protected override void Update(CanvasContext2D context)
        {
            context.ClearRect(0, 0, 800, 600);
            if (!_finished && (_nextLetter < Ticks || _keyPressed))
            {
                _textElement.InnerHTML += GetNextLetter();
                if (_nextLetter == 0 || _keyPressed) _nextLetter = Ticks + 100;
                else _nextLetter += 100;
            }
        }

        private string GetNextLetter()
        {
            string paragraphString = string.Empty;

            if (_text[_paragraphIndex].Length <= _charIndex)
            {
                paragraphString += "<br />";
                _charIndex = 0;
                _paragraphIndex++;
            }

            if (_text.Length <= _paragraphIndex)
            {
                _finished = true;
                return string.Empty;
            }

            return paragraphString + _text[_paragraphIndex].Substr(_charIndex++, 1);
        }

        protected override void KeyDown(ElementEvent e)
        {
            _keyPressed = true;
            if (_finished) _game.NextLevel();
        }

        protected override void KeyUp(ElementEvent e)
        {
            _keyPressed = false;
        }
    }
}
