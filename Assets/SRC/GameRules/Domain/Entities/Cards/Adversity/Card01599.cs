using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01599 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Weakness, Tag.Madness };
        private Investigator InvestigatorAffected => _investigatorsProvider.GetInvestigatorWithThisZone(CurrentZone);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<HarmToCardGameAction>(TakeDirectDamageConditionn, TakeDirectDamageLogic, GameActionTime.After);
            CreateActivation(2, DiscardLogic, DiscardCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private bool DiscardCondition(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Danger) return false;
            if ((investigator.CurrentPlace != ControlOwner.CurrentPlace)) return false;
            return true;
        }

        private async Task DiscardLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Start();
        }

        /*******************************************************************/
        private async Task TakeDirectDamageLogic(HarmToCardGameAction action)
        {
            await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(InvestigatorAffected, this, amountDamage: 1, isDirect: true).Start();
        }

        private bool TakeDirectDamageConditionn(HarmToCardGameAction harmToCardGameAction)
        {
            if (!IsInPlay) return false;
            if (harmToCardGameAction.Card != InvestigatorAffected.InvestigatorCard) return false;
            if (harmToCardGameAction.AmountFear < 1) return false;
            return true;
        }
    }
}
