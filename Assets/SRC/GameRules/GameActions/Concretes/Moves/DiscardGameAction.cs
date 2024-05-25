using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DiscardGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Card Card { get; }

        /*******************************************************************/
        public DiscardGameAction(Card card)
        {
            Card = card;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(Card, GetDiscardZone()));
            await ResetStats();
        }

        private async Task ResetStats()
        {
            List<Stat> statsWithValues = new();
            if (Card is IDamageable damageable) statsWithValues.Add(damageable.Health);
            if (Card is IFearable fearable) statsWithValues.Add(fearable.Sanity);
            if (Card is IChargeable chargeable) statsWithValues.Add(chargeable.AmountCharges);
            if (Card is IBulletable bulletable) statsWithValues.Add(bulletable.AmountBullets);
            if (Card is ISupplietable supplietable) statsWithValues.Add(supplietable.AmountSupplies);
            await _gameActionsProvider.Create(new ResetStatGameAction(statsWithValues));
        }

        private Zone GetDiscardZone()
        {
            if (Card.IsVictory) return _chaptersProvider.CurrentScene.VictoryZone;

            if (_chaptersProvider.CurrentScene.StartDeckDangerCards.Contains(Card))
                return _chaptersProvider.CurrentScene.DangerDiscardZone;

            return Card.Owner?.DiscardZone ?? _chaptersProvider.CurrentScene.OutZone;
        }
    }
}
