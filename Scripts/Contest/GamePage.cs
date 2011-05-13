using jQueryApi;
using System.Runtime.CompilerServices;

namespace Vtj.Contest
{
    internal static class GamePage
    {
        static GamePage()
        {
            jQuery.OnDocumentReady(delegate()
            {
                SpaceDinosGame game = new SpaceDinosGame();
                game.Start();
            });
        }
    }
}
