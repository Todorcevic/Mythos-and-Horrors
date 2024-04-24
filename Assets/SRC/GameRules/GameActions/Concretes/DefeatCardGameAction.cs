using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DefeatCardGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Card Card { get; }

        /*******************************************************************/
        public DefeatCardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DiscardGameAction(Card));
            Dictionary<Stat, int> statsWithValues = new();
            if (Card is IDamageable damageable) statsWithValues.Add(damageable.Health, Card.Info.Health ?? 0);
            if (Card is IFearable fearable) statsWithValues.Add(fearable.Sanity, Card.Info.Sanity ?? 0);
            await _gameActionsProvider.Create(new UpdateStatGameAction(statsWithValues));
        }
    }
}
