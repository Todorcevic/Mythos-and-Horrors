using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01154 : CardPlace
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Woods };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<InvestigatePlaceGameAction>(ChallengeWithPowerLogic, ChallengeWithPowerCondition, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task ChallengeWithPowerCondition(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            investigatePlaceGameAction.ChangeStat(investigatePlaceGameAction.ActiveInvestigator.Strength);
            await Task.CompletedTask;
        }

        private bool ChallengeWithPowerLogic(InvestigatePlaceGameAction investigatePlaceGameAction)
        {
            if (investigatePlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
