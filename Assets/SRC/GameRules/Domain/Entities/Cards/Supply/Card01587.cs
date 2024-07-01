using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01587 : CardSupply, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Tool };
        public Charge Charge { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Charge = new Charge(3, ChargeType.Supplie);
            CreateActivation(1, InvestigateLogic, InvestigateCondition, PlayActionType.Investigate);
        }

        /*******************************************************************/
        private bool InvestigateCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Charge.Amount.Value < 1) return false;
            if (investigator != ControlOwner) return false;
            return true;
        }

        private async Task InvestigateLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Charge.Amount, 1).Start();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(investigator.CurrentPlace.Enigma, 2).Start();
            await _gameActionsProvider.Create(new InvestigatePlaceGameAction(investigator, investigator.CurrentPlace));
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(investigator.CurrentPlace.Enigma, 2).Start();

        }
    }
}
