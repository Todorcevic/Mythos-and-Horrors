﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01508 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Slot ExtraTome1 { get; private set; }
        public Slot ExtraTome2 { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraTome1 = new Slot(SlotType.Item, SlotCondition);
            ExtraTome2 = new Slot(SlotType.Item, SlotCondition);
            CreateBuff(CardsToBuff, ActivationLogic, Deactivationlogic, new Localization("Buff_Card01508"));

            bool SlotCondition()
            {
                if (!ControlOwner.CardsInPlay.Any(card => card.HasThisTag(Tag.Tome))) return false;
                return true;
            }
        }

        /*******************************************************************/
        private IEnumerable<Card> CardsToBuff()
        {
            return IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : Enumerable.Empty<Card>();
        }

        private async Task ActivationLogic(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create<AddSlotGameAction>().SetWith(ControlOwner, ExtraTome1).Execute();
            await _gameActionsProvider.Create<AddSlotGameAction>().SetWith(ControlOwner, ExtraTome2).Execute();
        }

        private async Task Deactivationlogic(IEnumerable<Card> cards)
        {
            await _gameActionsProvider.Create<RemoveSlotGameAction>().SetWith(ControlOwner, ExtraTome1).Execute();
            await _gameActionsProvider.Create<RemoveSlotGameAction>().SetWith(ControlOwner, ExtraTome2).Execute();
        }

        /*******************************************************************/

    }
}
