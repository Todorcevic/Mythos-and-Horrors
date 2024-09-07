using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01180 : CardCreature, IStalker, ICounterAttackable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Humanoid };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<DefeatCardGameAction>(TakeFearCondition, TakeFearLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task TakeFearLogic(DefeatCardGameAction action)
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigaotrsAffected, TakeFear).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigaotrsAffected() => _investigatorsProvider.GetInvestigatorsInThisPlace(CurrentPlace);
            async Task TakeFear(Investigator investigator) => await _gameActionsProvider.Create<HarmToInvestigatorGameAction>()
                .SetWith(investigator, this, amountFear: 1).Execute();
        }

        private bool TakeFearCondition(DefeatCardGameAction defeatCardGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (defeatCardGameAction.Card != this) return false;
            return true;
        }
    }
}
