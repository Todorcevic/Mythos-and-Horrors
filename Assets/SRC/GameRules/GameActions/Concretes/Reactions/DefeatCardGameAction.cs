using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DefeatCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Card Card { get; }
        public Card ByThisCard { get; }
        public Investigator ByThisInvestigator => ByThisCard?.Owner;

        /*******************************************************************/
        public DefeatCardGameAction(Card card, Card byThisCard)
        {
            Card = card;
            ByThisCard = byThisCard;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (Card is CardInvestigator cardInvestigator)
            {
                await _gameActionsProvider.Create(new EliminateInvestigatorGameAction(cardInvestigator.Owner));
                await _gameActionsProvider.Create(new UpdateStatesGameAction(cardInvestigator.Defeated, true));
            }
            else await _gameActionsProvider.Create(new DiscardGameAction(Card));
        }
    }
}
