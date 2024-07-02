using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01684 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spirit };
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not HarmToInvestigatorGameAction harmToInvestigateGameAction) return;
            int amountResourcesTogain = Math.Max(0, 5 - harmToInvestigateGameAction.AmountDamage) + Math.Max(0, 5 - harmToInvestigateGameAction.AmountFear);

            harmToInvestigateGameAction.AddAmountDamage(-5);
            harmToInvestigateGameAction.AddAmountFear(-5);

            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, amountResourcesTogain).Execute();
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not HarmToInvestigatorGameAction harmToInvestigateGameAction) return false;
            if (harmToInvestigateGameAction.Investigator != ControlOwner) return false;
            return true;
        }

    }
}
