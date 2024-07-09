using System;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DrawAidGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public DrawAidGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Investigator.CardAidToDraw == null) await _gameActionsProvider.Create<RestoreAidDeckGameAction>().SetWith(Investigator).Execute();
            await _gameActionsProvider.Create<DrawGameAction>().SetWith(Investigator, Investigator.CardAidToDraw).Execute();
            if (Investigator.CardAidToDraw == null) await _gameActionsProvider.Create<RestoreAidDeckGameAction>().SetWith(Investigator).Execute();

        }
    }
}
