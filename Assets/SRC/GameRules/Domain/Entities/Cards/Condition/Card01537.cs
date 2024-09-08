using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01537 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override bool IsFast => true;
        public override IEnumerable<Tag> Tags => new[] { Tag.Insight };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            await _gameActionsProvider.Create<GainKeyGameAction>().SetWith(investigator, investigator.CurrentPlace.Keys, 1).Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (ControlOwner.CurrentPlace.Keys.Value < 1) return false;
            return true;
        }
    }
}
