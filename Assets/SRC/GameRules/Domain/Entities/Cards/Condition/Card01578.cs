using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01578 : CardCondition
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override bool IsFast => false;
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Elude;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new SafeForeach<CardCreature>(AllCreatures, EvedaAction));

            /*******************************************************************/

            IEnumerable<CardCreature> AllCreatures() => investigator.CreaturesInSamePlace;
            async Task EvedaAction(CardCreature creature) => await _gameActionsProvider.Create(new EludeGameAction(creature));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (!ControlOwner.CreaturesInSamePlace.Any()) return false;
            return true;
        }
    }
}
