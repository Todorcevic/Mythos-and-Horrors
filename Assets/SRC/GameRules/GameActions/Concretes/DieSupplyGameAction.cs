using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class DieSupplyGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardSupply CardSupply { get; }

        /*******************************************************************/
        public DieSupplyGameAction(CardSupply cardSupply)
        {
            CardSupply = cardSupply;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new DiscardGameAction(CardSupply));
            Dictionary<Stat, int> statsWithValues = new();
            if (CardSupply is IDamageable damageable) statsWithValues.Add(damageable.Health, CardSupply.Info.Health ?? 0);
            if (CardSupply is IFearable fearable) statsWithValues.Add(fearable.Sanity, CardSupply.Info.Sanity ?? 0);
            await _gameActionsProvider.Create(new UpdateStatGameAction(statsWithValues));
        }
    }
}
