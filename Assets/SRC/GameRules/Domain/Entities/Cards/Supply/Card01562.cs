﻿using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01562 : CardChallengeSupply
    {
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainIntelligenceSkillCondition, PlayActionType.Activate, new Localization("Activation_Card01562"));
            CreateChallengeActivation(GainSkillLogic, GainPowerSkillCondition, PlayActionType.Activate, new Localization("Activation_Card01562-1"));
        }

        /*******************************************************************/
        private bool GainPowerSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Power) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private bool GainIntelligenceSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Intelligence) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }
    }
}
