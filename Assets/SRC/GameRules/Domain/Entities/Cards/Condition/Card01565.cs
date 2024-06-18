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
            if (moveCardsGameAction.Parent is not PlayDrawActivableGameAction playRevelationAdversity) return false;
            if (playRevelationAdversity.Investigator != ControlOwner) return false;
            if (((Card)playRevelationAdversity.DrawActivable).HasThisTag(Tag.Weakness)) return false;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            if (gameAction is not MoveCardsGameAction moveCardsGameAction) return;
            if (moveCardsGameAction.Parent is not PlayDrawActivableGameAction playRevelationAdversity) return;
            playRevelationAdversity.Cancel();
            await _gameActionsProvider.Create(new DiscardGameAction((Card)playRevelationAdversity.DrawActivable));
            await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountFear: 1));
        }
    }
}