﻿using System.Diagnostics.CodeAnalysis;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01549 : CardChallengeSupply
    {
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateChallengeActivation(GainSkillLogic, GainAgilitySkillCondition, PlayActionType.Activate, new Localization("Activation_Card01549"));
            CreateChallengeActivation(GainSkillLogic, GainStrengthSkillCondition, PlayActionType.Activate, new Localization("Activation_Card01549-1"));
        }

        /*******************************************************************/
        private bool GainStrengthSkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Strength) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }

        private bool GainAgilitySkillCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (IsInPlay.IsFalse) return false;
            if (challengePhaseGameAction.Stat != ControlOwner.Agility) return false;
            if (ControlOwner.Resources.Value <= 0) return false;
            return true;
        }
    }
}
