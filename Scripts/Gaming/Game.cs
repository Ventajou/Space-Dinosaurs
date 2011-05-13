using System.Runtime.CompilerServices;

namespace Vtj.Gaming
{
    internal abstract class Game
    {
        #region Private Members
        private Scene _currentScene;
        private int _levelIndex = -1;
        #endregion

        #region Abstract Properties
        protected abstract Scene TitleScene { get; }
        protected abstract Scene HighScoresScene { get; }
        protected abstract Scene GameOverScene { get; }
        protected abstract Scene EndScene { get; }
        protected abstract Scene[] Levels { get; }
        #endregion

        public int Score;

        public void Start()
        {
            ShowTitleScreen();
        }

        public void Play()
        {
            _levelIndex = 0;
            Score = 0;
            LoadScene(Levels[_levelIndex]);
        }

        public void NextLevel()
        {
            if (_levelIndex++ >= Levels.Length)
            {
                ShowEnd();
                return;
            }

            LoadScene(Levels[_levelIndex]);
        }

        public void ShowTitleScreen()
        {
            LoadScene(TitleScene);
        }

        public void ShowHiScores()
        {
            LoadScene(HighScoresScene);
        }

        public void GameOver()
        {
            LoadScene(GameOverScene);
        }

        public void ShowEnd()
        {
            LoadScene(EndScene);
        }

        public void LoadScene(Scene scene)
        {
            if (_currentScene != null)
            {
                _currentScene.Dispose();
            }

            _currentScene = scene;
            scene.Start();
        }
    }
}
