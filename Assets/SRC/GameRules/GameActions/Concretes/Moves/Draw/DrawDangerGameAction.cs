using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DrawDangerGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public DrawDangerGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Investigator.CardDangerToDraw == null) await _gameActionsProvider.Create<RestoreDangerDeckGameAction>().SetWith(Investigator).Execute();
            await _gameActionsProvider.Create<DrawGameAction>().SetWith(Investigator, Investigator.CardDangerToDraw).Execute();
            if (Investigator.CardDangerToDraw == null) await _gameActionsProvider.Create<RestoreDangerDeckGameAction>().SetWith(Investigator).Execute();

        }
    }
}
