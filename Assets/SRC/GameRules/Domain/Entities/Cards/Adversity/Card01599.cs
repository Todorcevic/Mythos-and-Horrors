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
            CreateReaction<HarmToCardGameAction>(TakeDirectDamageConditionn, TakeDirectDamageLogic, GameActionTime.After);
            CreateActivation(CreateStat(2), DiscardLogic, DiscardCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayAdversityFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private bool DiscardCondition(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Danger) return false;
            if ((investigator.CurrentPlace != ControlOwner.CurrentPlace)) return false;
            return true;
        }

        private async Task DiscardLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        /*******************************************************************/
        private async Task TakeDirectDamageLogic(HarmToCardGameAction action)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(InvestigatorAffected, this, amountDamage: 1, isDirect: true));
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
