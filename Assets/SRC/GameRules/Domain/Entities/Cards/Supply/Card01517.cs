using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01517 : CardChallengeSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainPowerSkillCondition, PlayActionType.Activate);
            CreateChallengeActivation(GainSkillLogic, GainStregnthSkillCondition, PlayActionType.Activate);
        }

        /*******************************************************************/
        private bool GainStregnthSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Strength) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private bool GainPowerSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Power) return false;
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
