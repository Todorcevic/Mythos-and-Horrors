﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01115 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ResignActivate, ResignConditionToActivate, PlayActionType.Resign, new Localization("Activation_Card01115"));
            CanMoveHere = new Conditional(() => IsInPlay.IsTrue && Revealed.IsActive);
        }

        /*******************************************************************/
        private async Task ResignActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<RunAwayGameAction>().SetWith(activeInvestigator).Execute();
        }

        private bool ResignConditionToActivate(Investigator activeInvestigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (activeInvestigator.CurrentPlace != this) return false;
            return true;
        }
    }
}
