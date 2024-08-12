using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01577 : CardChallengeSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainAgilitySkillCondition, PlayActionType.Activate, "Activation_Card01577");
            CreateChallengeActivation(GainSkillLogic, GainPowerSkillCondition, PlayActionType.Activate, "Activation_Card01577-1");
        }

        /*******************************************************************/

        private bool GainPowerSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Power) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private bool GainAgilitySkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Agility) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }
    }
}
