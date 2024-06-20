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
            await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 3));
            await _gameActionsProvider.Create(new DrawAidGameAction(investigator));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
