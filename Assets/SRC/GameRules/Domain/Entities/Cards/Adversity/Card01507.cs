using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01507 : CardAdversity, IResetable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Task };
        public Stat Hints { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraStat = Hints = CreateStat(3);
            CreateOptativeReaction<GainHintGameAction>(PayHintCondition, PayHintLogic, isAtStart: false);
            CreateReaction<FinalizeGameAction>(TakeShockCondition, TakeShockLogic, isAtStart: true);
        }

        /*******************************************************************/
        private async Task TakeShockLogic(FinalizeGameAction finalizeGameAction)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(ControlOwner.Shock, 1));
        }

        private bool TakeShockCondition(FinalizeGameAction finalizeGameAction)
        {
            if (!IsInPlay) return false;
            if (Hints.Value <= 0) return false;
            return true;
        }

        /*******************************************************************/
        private async Task PayHintLogic(GainHintGameAction gainHintGameAction)
        {
            await _gameActionsProvider.Create(new PayHintGameAction(gainHintGameAction.Investigator, Hints, gainHintGameAction.Amount));
        }

        private bool PayHintCondition(GainHintGameAction gainHintGameAction)
        {
            if (!IsInPlay) return false;
            if (gainHintGameAction.Investigator.CurrentPlace != ControlOwner.CurrentPlace) return false;
            if (Hints.Value <= 0) return false;
            return true;
        }

        public async Task Reset()
        {
            await _gameActionsProvider.Create(new UpdateStatGameAction(Hints, Hints.InitialValue));
        }
    }
}
