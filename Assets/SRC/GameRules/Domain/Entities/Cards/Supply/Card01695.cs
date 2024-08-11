using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01695 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Slot ExtraTrinket { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraTrinket = new Slot(SlotType.Trinket);
            CreateBuff(CardToBuff, ActiveBuff, DeactiveBuff, "Buff_Card01695");
        }

        /*******************************************************************/
        private async Task ActiveBuff(IEnumerable<Card> enumerable)
        {
            await _gameActionsProvider.Create<AddSlotGameAction>().SetWith(ControlOwner, ExtraTrinket).Execute();
        }

        private async Task DeactiveBuff(IEnumerable<Card> enumerable)
        {
            await _gameActionsProvider.Create<RemoveSlotGameAction>().SetWith(ControlOwner, ExtraTrinket).Execute();
        }

        private IEnumerable<Card> CardToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();

    }
}
