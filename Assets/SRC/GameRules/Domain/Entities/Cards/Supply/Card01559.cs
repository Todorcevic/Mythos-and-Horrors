using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01559 : CardSupply, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Charm };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);
            CreateBuff(CardToSelect, BuffOn, BuffOff);
        }

        /*******************************************************************/
        private async Task BuffOn(IEnumerable<Card> cardsToBuff)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(ControlOwner.Power, 1).Start();
        }

        private async Task BuffOff(IEnumerable<Card> cardsToDebuff)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(ControlOwner.Power, 1).Start();
        }

        private IEnumerable<Card> CardToSelect() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
