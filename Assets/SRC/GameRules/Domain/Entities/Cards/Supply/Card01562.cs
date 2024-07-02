using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01562 : CardChallengeSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainIntelligenceSkillCondition, PlayActionType.Activate);
            CreateChallengeActivation(GainSkillLogic, GainPowerSkillCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool GainPowerSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Power) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private bool GainIntelligenceSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Intelligence) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private async Task GainSkillLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(ControlOwner, 1).Execute();
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(challengePhaseGameAction.StatModifier, 1).Execute();
        }

    }
}
