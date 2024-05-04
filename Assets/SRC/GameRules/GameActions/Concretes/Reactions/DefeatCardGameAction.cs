using System.Collections.Generic;
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
            else if (Card.IsVictory) await _gameActionsProvider.Create(new MoveCardsGameAction(Card, _chaptersProvider.CurrentScene.VictoryZone));
            else await _gameActionsProvider.Create(new DiscardGameAction(Card));

            Dictionary<Stat, int> statsWithValues = new();
            if (Card is IDamageable damageable) statsWithValues.Add(damageable.Health, Card.Info.Health ?? 0);
            if (Card is IFearable fearable) statsWithValues.Add(fearable.Sanity, Card.Info.Sanity ?? 0);
            await _gameActionsProvider.Create(new UpdateStatGameAction(statsWithValues));
        }
    }
}
