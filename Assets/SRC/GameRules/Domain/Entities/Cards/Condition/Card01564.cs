using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01564 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<DrawDangerGameAction>().SetWith(investigator).Start();
            await _gameActionsProvider.Create<GainHintGameAction>().SetWith(investigator, investigator.CurrentPlace.Hints, 2).Start();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;

    }
}
