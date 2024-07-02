using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01132 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateBuff(CardsToBuff, ActivationLogic, DeactivationLogic);
        }

        /*******************************************************************/
        private async Task ActivationLogic(IEnumerable<Card> cardsToBuff)
        {
            Dictionary<Stat, int> allStats = cardsToBuff.OfType<CardSupply>().ToDictionary(supply => supply.ResourceCost, supply => 2);
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(allStats).Execute();
        }

        private async Task DeactivationLogic(IEnumerable<Card> cardsToDebuff)
        {
            Dictionary<Stat, int> allStats = cardsToDebuff.OfType<CardSupply>().ToDictionary(supply => supply.ResourceCost, supply => 2);
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(allStats).Execute();
        }

        private IEnumerable<Card> CardsToBuff()
        {
            return InvestigatorsInThisPlace.SelectMany(investigator => investigator.Zones.SelectMany(zones => zones.Cards)).OfType<CardSupply>()
                .Where(card => card.HasThisTag(Tag.Ally));
        }
    }
}
