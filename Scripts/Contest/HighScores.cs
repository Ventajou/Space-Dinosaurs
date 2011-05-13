using System.Html;
using System.Runtime.CompilerServices;
using System.Serialization;
using jQueryApi;
using Vtj.Gaming;

namespace Vtj.Contest
{
    internal class HighScores : Scene
    {
        Element _overlay = jQuery.Select(".GameOverlay").GetElement(0);

        public HighScores(Game game)
            : base(game)
        {
            ShowWaitMessage = false;
        }

        protected override void Init()
        {
            AddSystem(new MenuBackgroundSystem());

            CanPause = false;

            BuildMenuButton("Back", 530, delegate(ElementEvent e)
                {
                    CurrentGame.ShowTitleScreen();
                });

            Element title = BuildMenuButton("High Scores", 80, null);
            title.ClassName += " MenuTitle";

            HighScore[] scores = (HighScore[])Json.Parse((string)Window.LocalStorage[SpaceDinosGame.HighScoresKey]);

            TableElement table = (TableElement)Document.CreateElement("TABLE");
            table.ClassName = "HighScores";
            _overlay.AppendChild(table);
            Element tableBody = Document.CreateElement("TBODY");
            table.AppendChild(tableBody);

            TableRowElement row = (TableRowElement)Document.CreateElement("TR");
            tableBody.AppendChild(row);

            AddCell(row, "TH", string.Empty);
            AddCell(row, "TH", "Score");
            AddCell(row, "TH", "Level");
            AddCell(row, "TH", "Name");

            for (int i = 0; i < 10; i++)
            {
                row = (TableRowElement)Document.CreateElement("TR");
                tableBody.AppendChild(row);

                switch (i)
                {
                    case 0:
                        AddCell(row, "TD", "1st");
                        break;
                    case 1:
                        AddCell(row, "TD", "2nd");
                        break;
                    case 2:
                        AddCell(row, "TD", "3rd");
                        break;
                    default:
                        AddCell(row, "TD", (i + 1) + "th");
                        break;
                }

                AddCell(row, "TD", scores[i].Score.ToString());
                AddCell(row, "TD", scores[i].Level.ToString());
                AddCell(row, "TD", scores[i].Name);
            }

            jQuery.FromElement(_overlay).Show();
        }

        private Element BuildMenuButton(string label, int position, ElementEventListener clickListener)
        {
            Element wrapper = Document.CreateElement("DIV");
            _overlay.AppendChild(wrapper);
            wrapper.ClassName = "MenuButton";
            wrapper.Style.Top = position + "px";
            Element button = Document.CreateElement("SPAN");
            wrapper.AppendChild(button);
            button.InnerHTML = label;
            if (clickListener != null) button.AddEventListener("click", clickListener, false);
            return wrapper;
        }

        private void AddCell(TableRowElement row, string tagName, string label)
        {
            Element cell = (TableCellElement)Document.CreateElement(tagName);
            row.AppendChild(cell);
            cell.InnerHTML = label;
        }
    }
}
