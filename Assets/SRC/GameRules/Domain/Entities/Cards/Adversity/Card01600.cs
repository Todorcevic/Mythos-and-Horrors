using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01600 : CardAdversity
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
            CreateReaction<HarmToCardGameAction>(TakeDirectFearConditionn, TakeDirectFearLogic, isAtStart: false);
            CreateActivation(CreateStat(2), DiscardLogic, DiscardCondition);
        }

        /*******************************************************************/
        private bool DiscardCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private async Task DiscardLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        /*******************************************************************/
        private async Task TakeDirectFearLogic(HarmToCardGameAction action)
        {
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(InvestigatorAffected, this, amountFear: 1, isDirect: true));
        }

        private bool TakeDirectFearConditionn(HarmToCardGameAction harmToCardGameAction)
        {
            if (!IsInPlay) return false;
            if (harmToCardGameAction.Card != InvestigatorAffected.InvestigatorCard) return false;
            if (harmToCardGameAction.AmountDamage < 1) return false;
            return true;

        }
    }
}
