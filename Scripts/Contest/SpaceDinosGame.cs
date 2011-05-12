using System.Html;
using System.Runtime.CompilerServices;
using System.Serialization;
using Vtj.Contest.Race;
using Vtj.Contest.Shooter;
using Vtj.Gaming;

namespace Vtj.Contest
{
    [ScriptNamespace("vtsd")]
    internal class SpaceDinosGame : Game
    {
        public const string HighScoresKey = "HighScores";

        private Scene _title;
        private Scene _highScores;
        private Scene _gameOver;
        private Scene _end;
        private Scene[] _levels;

        public SpaceDinosGame()
        {
            EnsureDefaultHighScores();
            _title = new Title(this);
            _highScores = new HighScores(this);
            _gameOver = new GameOver(this);
            _end = new End(this);
            _levels = new Scene[] { 
                new CutScene(this, "Images/CutScenes/1_1.png", Strings.CutScene_1_1),
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_1_2),
                new CutScene(this, "Images/CutScenes/1_3.png", Strings.CutScene_1_3),
                new CutScene(this, "Images/CutScenes/1_4.png", Strings.CutScene_1_4),
                new CutScene(this, "Images/CutScenes/1_5.png", Strings.CutScene_1_5),
                new CutScene(this, "Images/CutScenes/1_6.png", Strings.CutScene_1_6),
                new RaceLevel(this, false, 4000000), 
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_2_1),
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_2_2),      
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_2_3),
                new ShooterLevel(this, false, 15000), 
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_3_1),
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_3_2),
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_3_3),
                new RaceLevel(this, false, 5000000),
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_4_1),
                new ShooterLevel(this, false, 20000),
                new CutScene(this, "Images/CutScenes/1_2.png", Strings.CutScene_5_1)};
        }

        protected override Scene TitleScene
        {
            get { return _title; }
        }

        protected override Scene HighScoresScene
        {
            get { return _highScores; }
        }

        protected override Scene GameOverScene
        {
            get { return _gameOver; }
        }

        protected override Scene EndScene
        {
            get { return _end; }
        }

        protected override Scene[] Levels
        {
            get { return _levels; }
        }

        private void EnsureDefaultHighScores()
        {
            if (Window.LocalStorage[HighScoresKey] != null) return;

            HighScore[] scores = new HighScore[10];

            for (int i = 0; i < 10; i++)
            {
                scores[i] = (HighScore)new object();
                scores[i].Name = "VTJ";
                scores[i].Score = 10000 - i * 1000;
                scores[i].Level = 1;
            }

            Window.LocalStorage[HighScoresKey] = Json.Stringify(scores);
        }
    }
}
