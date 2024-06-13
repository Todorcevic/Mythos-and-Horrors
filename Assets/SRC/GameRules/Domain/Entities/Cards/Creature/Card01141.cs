using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01141 : CardCreature, ISpawnable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };
        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public CardPlace SpawnPlace => SceneCORE2.Hospital;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<EludeCreatureGameAction>(EludeCondition, EludeLogic, isAtStart: false);
        }

        /*******************************************************************/
        private async Task EludeLogic(EludeCreatureGameAction eludeGameAction)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, _chaptersProvider.CurrentScene.VictoryZone));
        }

        private bool EludeCondition(EludeCreatureGameAction aludeGameAction)
        {
            if (aludeGameAction.CardCreature != this) return false;
            return true;
        }
    }
}
