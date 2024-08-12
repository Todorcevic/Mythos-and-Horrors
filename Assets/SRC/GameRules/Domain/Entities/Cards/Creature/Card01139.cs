using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01139 : CardCreature, ISpawnable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        private SceneCORE2 SceneCORE2 => (SceneCORE2)_chaptersProvider.CurrentScene;
        public CardPlace SpawnPlace => SceneCORE2.University;
        public override IEnumerable<Tag> Tags => new[] { Tag.Humanoid, Tag.Cultist, Tag.Miskatonic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ParleyActivate, ParleyConditionToActivate, PlayActionType.Parley, "Activation_Card01139");
        }

        /*******************************************************************/
        private bool ParleyConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (CurrentPlace != investigator.CurrentPlace) return false;
            if (!investigator.CanPayHints.IsActive) return false;
            if (investigator.Hints.Value < 2) return false;
            return true;
        }

        private async Task ParleyActivate(Investigator investigator)
        {
            await _gameActionsProvider.Create<ParleyGameAction>().SetWith(PayCreature).Execute();

            /*******************************************************************/
            async Task PayCreature()
            {
                await _gameActionsProvider.Create<PayHintGameAction>().SetWith(investigator, Agility, 2).Execute();
                await _gameActionsProvider.Create<MoveCardsGameAction>()
                    .SetWith(this, _chaptersProvider.CurrentScene.VictoryZone).Execute();
            }
        }
    }
}
