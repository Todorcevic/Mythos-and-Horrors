using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01517 : CardChallengeSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainPowerSkillCondition, PlayActionType.Activate, new Localization("Activation_Card01517"));
            CreateChallengeActivation(GainSkillLogic, GainStregnthSkillCondition, PlayActionType.Activate, new Localization("Activation_Card01517-1"));
        }

        /*******************************************************************/
        private bool GainStregnthSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Strength) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private bool GainPowerSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Power) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }
    }
}
