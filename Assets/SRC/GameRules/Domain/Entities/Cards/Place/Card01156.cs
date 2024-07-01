using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Zenject;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace MythosAndHorrors.GameRules
{
    public class Card01156 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Cave };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<RoundGameAction>(RestoreHintsLogic, RestoreHintsCondition, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task RestoreHintsCondition(RoundGameAction action)
        {
            await _gameActionsProvider.Create<UpdateStatGameAction>().SetWith(Hints, _investigatorsProvider.AllInvestigators.Count() * 2).Start();
        }

        private bool RestoreHintsLogic(RoundGameAction action)
        {
            if (Hints.Value >= _investigatorsProvider.AllInvestigators.Count() * 2) return false;
            return true;
        }
    }
}
