using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01693 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Supply };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainResourceGameAction>().SetWith(investigator, 3).Execute();
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator) => true;

    }
}
