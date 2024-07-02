using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01124 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public State Played { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Arkham };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Played = CreateState(false);
            CreateActivation(1, DrawCardAndResourceLogic, DrawCardAndResourceCondition, PlayActionType.Activate);
            CreateForceReaction<PlayInvestigatorGameAction>(ResetPlayedCondition, ResetPlayedLogic, GameActionTime.After);
            CreateForceReaction<SpawnCreatureGameAction>(PriestSpawnCondition, PriestSpawnLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task PriestSpawnLogic(SpawnCreatureGameAction spawnCreatureGameAction)
        {
            spawnCreatureGameAction.SetWith(spawnCreatureGameAction.Creature, this);
            await Task.CompletedTask;
        }

        private bool PriestSpawnCondition(SpawnCreatureGameAction spawnCreatureGameAction)
        {
            if (spawnCreatureGameAction.Creature is not Card01116) return false;
            return true;
        }

        /*******************************************************************/
        private bool DrawCardAndResourceCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace != this) return false;
            if (Played.IsActive) return false;
            return true;
        }

        private async Task DrawCardAndResourceLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<DrawAidGameAction>().SetWith(investigator).Start();
            await _gameActionsProvider.Create(new GainResourceGameAction(investigator, 1));
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, true).Start();
        }

        private async Task ResetPlayedLogic(PlayInvestigatorGameAction action) =>
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Played, false).Start();


        private bool ResetPlayedCondition(PlayInvestigatorGameAction action) => true;

    }
}
