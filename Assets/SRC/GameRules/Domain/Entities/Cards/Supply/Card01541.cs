using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01541 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Item, Tag.Relic };

        /*******************************************************************/
        [Inject]
        public void Init()
        {
            CreateOptativeReaction<SpawnCreatureGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task Logic(SpawnCreatureGameAction spawnCreatureGameAction)
        {
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
            await _gameActionsProvider.Create<DiscardGameAction>().SetWith(spawnCreatureGameAction.Creature).Execute();
        }

        private bool Condition(SpawnCreatureGameAction spawnCreatureGameAction)
        {
            if (!IsInPlay) return false;
            if (spawnCreatureGameAction.Place != ControlOwner.CurrentPlace) return false;
            if (spawnCreatureGameAction.Creature.HasThisTag(Tag.Elite)) return false;
            return true;
        }

        /*******************************************************************/
    }
}
