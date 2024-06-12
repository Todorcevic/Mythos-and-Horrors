using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01579 : CardConditionTrigged
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Fortune };
        protected override bool IsFast => true;
        protected override bool FastReactionAtStart => false;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 2));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not InvestigateGameAction investigateGameAction) return false;
            if (investigateGameAction.ActiveInvestigator != ControlOwner) return false;
            if (investigateGameAction.IsSuccessful ?? true) return false;
            if (investigateGameAction.CurrentTotalChallengeValue < -2) return false;
            if (ControlOwner.CurrentPlace.Hints.Value < 1) return false;
            return true;
        }

    }
}
