using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01694 : CardSupply, IPermanentable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Slot ExtraAlly { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraAlly = new Slot(SlotType.Supporter);
            CreateBuff(CardToBuff, ActiveBuff, DeactiveBuff);
        }

        /*******************************************************************/
        private async Task ActiveBuff(IEnumerable<Card> enumerable)
        {
            await _gameActionsProvider.Create(new AddSlotGameAction(ControlOwner, ExtraAlly));
        }

        private async Task DeactiveBuff(IEnumerable<Card> enumerable)
        {
            await _gameActionsProvider.Create(new RemoveSlotGameAction(ControlOwner, ExtraAlly));
        }

        private IEnumerable<Card> CardToBuff() => IsInPlay ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
    }
}
