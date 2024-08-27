using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class RestoreAidDeckGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public override bool CanBeExecuted => Investigator.IsInPlay.IsTrue;

        /*******************************************************************/
        public RestoreAidDeckGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(Investigator.DiscardZone.Cards, Investigator.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(Investigator.DeckZone).Execute();
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(Investigator, null, amountFear: 1).Execute();
        }
    }
}
