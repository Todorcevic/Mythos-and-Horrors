using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01684 : CardConditionFast
    {
        private int _amountResourcesTogain;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spirit };
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;
        protected override string LocalizableCode => "OptativeReaction_Card01684";
        protected override string[] LocalizableArgs => new[] { _amountResourcesTogain.ToString(), _amountResourcesTogain.ToString() };

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
            _amountResourcesTogain = Math.Max(0, 5 - harmToInvestigateGameAction.AmountDamage) + Math.Max(0, 5 - harmToInvestigateGameAction.AmountFear);
            if (_amountResourcesTogain < 1) return false;
            return true;
        }

    }
}
