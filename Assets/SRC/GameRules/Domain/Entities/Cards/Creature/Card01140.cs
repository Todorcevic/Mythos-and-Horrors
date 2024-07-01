using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01140 : CardCreature, ISpawnable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist };
        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public CardPlace SpawnPlace => SceneCORE2.North;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley);
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (CurrentPlace != investigator.CurrentPlace) return false;
            if (investigator.Resources.Value < 5) return false;
            return true;
        }

        private async Task ParleyActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create<ParleyGameAction>().SetWith(PayCreature).Start();

            async Task PayCreature()
            {
                await _gameActionsProvider.Create(new PayResourceGameAction(investigator, 5));
                await _gameActionsProvider.Create<MoveCardsGameAction>()
                    .SetWith(this, _chaptersProvider.CurrentScene.VictoryZone).Start();
            }
        }
    }
}
