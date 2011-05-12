using jQueryApi;
using System.Runtime.CompilerServices;

namespace Vtj.Contest
{
    [ScriptNamespace("vtsd")]
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
