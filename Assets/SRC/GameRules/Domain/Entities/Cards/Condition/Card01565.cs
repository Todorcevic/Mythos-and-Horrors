using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01565 : CardConditionFast
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell, Tag.Spirit };
        protected override bool IsFast => true;
        protected override GameActionTime FastReactionAtStart => GameActionTime.After;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return false;
            if (moveCardsGameAction.Parent is not PlayRevelationAdversityGameAction playRevelationAdversity) return false;
            if (playRevelationAdversity.Investigator != ControlOwner) return false;
            if (playRevelationAdversity.CardAdversity.HasThisTag(Tag.Weakness)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return;
            if (moveCardsGameAction.Parent is not PlayRevelationAdversityGameAction playRevelationAdversity) return;
            playRevelationAdversity.Cancel();
            await _gameActionsProvider.Create(new DiscardGameAction(playRevelationAdversity.CardAdversity));
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountFear: 1));
        }
    }
}