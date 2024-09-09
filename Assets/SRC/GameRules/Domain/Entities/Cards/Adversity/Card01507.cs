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
        public Stat Keys { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Task };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Keys = CreateStat(3);
            Charge = new Charge(Keys, ChargeType.Secret);
            CreateOptativeReaction<GainKeyGameAction>(PayKeyCondition, PayKeyLogic, GameActionTime.Before, new Localization("OptativeReaction_Card01182"));
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
            if (IsInPlay.IsFalse) return false;
            if (Keys.Value <= 0) return false;
            return true;
        }

        /*******************************************************************/
        private async Task PayKeyLogic(GainKeyGameAction gainKeyGameAction)
        {
            gainKeyGameAction.Cancel();
            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(Keys, gainKeyGameAction.Amount).Execute();
        }

        private bool PayKeyCondition(GainKeyGameAction gainKeyGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (gainKeyGameAction.Investigator.CurrentPlace != ControlOwner.CurrentPlace) return false;
            if (Keys.Value <= 0) return false;
            return true;
        }

        public async Task Reset()
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(Keys, Keys.InitialValue).Execute();
        }
    }
}
