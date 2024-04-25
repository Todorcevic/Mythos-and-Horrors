using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DefeatCardGameAction : GameAction
    {
        private readonly Card _byThisCard;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Card Card { get; }
        public Card ByThisCard => _byThisCard ?? GetFromCard();

        /*******************************************************************/
        public DefeatCardGameAction(Card card, Card byThisCard = null)
        {
            Card = card;
            _byThisCard = byThisCard;
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

            Dictionary<Stat, int> statsWithValues = new();
            if (Card is IDamageable damageable) statsWithValues.Add(damageable.Health, Card.Info.Health ?? 0);
            if (Card is IFearable fearable) statsWithValues.Add(fearable.Sanity, Card.Info.Sanity ?? 0);
            await _gameActionsProvider.Create(new UpdateStatGameAction(statsWithValues));
        }

        private Card GetFromCard()
        {
            if (Parent is UpdateStatesGameAction updateGameAction && updateGameAction.Parent is HarmToCardGameAction harmToCardGameAction)
                return harmToCardGameAction.ByThisCard;

            return default;
        }
    }
}
