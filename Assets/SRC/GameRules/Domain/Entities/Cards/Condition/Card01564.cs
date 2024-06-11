using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01564 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };
        protected override bool IsFast => false;

        /*******************************************************************/

        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new DrawDangerGameAction(investigator));
            await _gameActionsProvider.Create(new GainHintGameAction(investigator, investigator.CurrentPlace.Hints, 2));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
