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

        public IReaction EnterPlayReaction { get; private set; }

        public CardPlace SpawnPlace => _chaptersProvider.CurrentScene.Info.PlaceCards
            .FirstOrDefault(place => place.IsInPlay && place.IsAlone);

        public override IEnumerable<Tag> Tags => new[] { Tag.Cultist, Tag.Humanoid };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            CreateReaction<MoveCardsGameAction>(EnterPlayCondition, EnterPlayLogic, isAtStart: false);
        }

        /*******************************************************************/
        private async Task EnterPlayLogic(MoveCardsGameAction creatureAttackGameAction)
        {
            await _gameActionsProvider.Create(new IncrementStatGameAction(Eldritch, 1));
        }

        private bool EnterPlayCondition(MoveCardsGameAction moveCardGameAction)
        {
            if (!moveCardGameAction.EnterPlayCardsAfter.Contains(this)) return false;
            if (!IsInPlay) return false;
            return true;
        }
    }

}
