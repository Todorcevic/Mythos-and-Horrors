﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01113 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => Enumerable.Empty<Tag>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(TakeFearCondition, TakeFearLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task TakeFearLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create<SafeForeach<Investigator>>().SetWith(InvestigaotrsAffected, TekeFear).Execute();

            /*******************************************************************/
            IEnumerable<Investigator> InvestigaotrsAffected() =>
                moveInvestigatorToPlaceGameAction.Investigators.Where(investigator => investigator.CurrentPlace == this);
            async Task TekeFear(Investigator investigator) =>
                await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigator, amountFear: 1, fromCard: this).Execute();
        }

        private bool TakeFearCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
