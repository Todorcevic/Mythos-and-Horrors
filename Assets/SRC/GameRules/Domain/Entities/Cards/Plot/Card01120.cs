﻿using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01120 : CardPlot
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        private Card01121 MaskedHunter => _cardsProvider.GetCard<Card01121>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            CreateActivation(1, RunAwayActivate, ResignConditionToActivate, PlayActionType.Resign, new Localization("Activation_Card01121a"));
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<DrawGameAction>().SetWith(_investigatorProvider.Leader, MaskedHunter).Execute();
        }

        /*******************************************************************/
        private async Task RunAwayActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<RunAwayGameAction>().SetWith(activeInvestigator).Execute();
        }

        private bool ResignConditionToActivate(Investigator activeInvestigator)
        {
            if (IsInPlay.IsFalse) return false;
            return true;
        }
    }
}
