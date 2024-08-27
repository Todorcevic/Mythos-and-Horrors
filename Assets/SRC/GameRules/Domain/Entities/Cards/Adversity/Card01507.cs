using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01507 : CardAdversity, IResetable, IChargeable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Charge Charge { get; private set; }
        public Stat Hints { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Task };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Hints = CreateStat(3);
            Charge = new Charge(Hints, ChargeType.Special);
            CreateOptativeReaction<GainHintGameAction>(PayHintCondition, PayHintLogic, GameActionTime.Before, "OptativeReaction_Card01182");
            CreateForceReaction<FinalizeGameAction>(TakeShockCondition, TakeShockLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private async Task TakeShockLogic(FinalizeGameAction finalizeGameAction)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(ControlOwner.Shock, 1).Execute();
        }

        private bool TakeShockCondition(FinalizeGameAction finalizeGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (Hints.Value <= 0) return false;
            return true;
        }

        /*******************************************************************/
        private async Task PayHintLogic(GainHintGameAction gainHintGameAction)
        {
            gainHintGameAction.Cancel();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Hints, gainHintGameAction.Amount).Execute();
        }

        private bool PayHintCondition(GainHintGameAction gainHintGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (gainHintGameAction.Investigator.CurrentPlace != ControlOwner.CurrentPlace) return false;
            if (Hints.Value <= 0) return false;
            return true;
        }

        public async Task Reset()
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(Hints, Hints.InitialValue).Execute();
        }
    }
}
