using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01534 : CardChallengeSupply
    {
        public override IEnumerable<Tag> Tags => new[] { Tag.Talent };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainAgilitySkillCondition, PlayActionType.Activate, "Activation_Card01534");
            CreateChallengeActivation(GainSkillLogic, GainIntelligenceSkillCondition, PlayActionType.Activate, "Activation_Card01534-1");
        }

        /*******************************************************************/
        private bool GainIntelligenceSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!IsInPlay) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Intelligence) return false;
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
