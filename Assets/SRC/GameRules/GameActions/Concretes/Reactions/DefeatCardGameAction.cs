using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DefeatCardGameAction : GameAction
    {
        public Card Card { get; private set; }
        public Card ByThisCard { get; private set; }
        public Investigator ByThisInvestigator => ByThisCard?.Owner;

        /*******************************************************************/
        public DefeatCardGameAction SetWith(Card card, Card byThisCard)
        {
            Card = card;
            ByThisCard = byThisCard;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Card is CardInvestigator cardInvestigator)
            {
                if (cardInvestigator.Owner.HealthLeft < 0) await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(cardInvestigator.Owner.Injury, 1).Execute();
                if (cardInvestigator.Owner.SanityLeft < 0) await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(cardInvestigator.Owner.Shock, 1).Execute();

                await _gameActionsProvider.Create<EliminateInvestigatorGameAction>().SetWith(cardInvestigator.Owner).Execute();
                await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(cardInvestigator.Defeated, true).Execute();
            }
            else await _gameActionsProvider.Create<DiscardGameAction>().SetWith(Card).Execute();
        }
    }
}
