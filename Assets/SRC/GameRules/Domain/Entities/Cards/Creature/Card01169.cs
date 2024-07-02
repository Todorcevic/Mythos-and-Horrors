using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01169 : CardCreature, ISpawnable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly ReactionablesProvider _reactionablesProvider;

        public CardPlace SpawnPlace => _chaptersProvider.CurrentScene.PlaceCards
            .FirstOrDefault(place => place.IsInPlay && place.IsAlone);
        public override IEnumerable<Tag> Tags => new[] { Tag.Cultist, Tag.Humanoid };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            CreateForceReaction<SpawnCreatureGameAction>(EnterPlayCondition, EnterPlayLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task EnterPlayLogic(SpawnCreatureGameAction spawnCreatureGameAction)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Eldritch, 1).Execute();
        }

        private bool EnterPlayCondition(SpawnCreatureGameAction spawnCreatureGameAction)
        {
            if (spawnCreatureGameAction.Creature != this) return false;
            if (!IsInPlay) return false;
            return true;
        }
    }

}
