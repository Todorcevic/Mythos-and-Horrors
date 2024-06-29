using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01568 : CardConditionFast
    {
        private IPhase _phase;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Spell };
        protected override GameActionTime FastReactionAtStart => GameActionTime.Before;

        /*******************************************************************/
        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (gameAction is not IPhase phase) return false;
            if (!ControlOwner.CreaturesInSamePlace.Any(creature => !creature.HasThisTag(Tag.Elite))) return false;
            _phase = phase;
            return true;
        }

        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction interactable = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Creature");
            interactable.CreateCancelMainButton(_gameActionsProvider);
            foreach (CardCreature creature in investigator.CreaturesInSamePlace.Where(creature => !creature.HasThisTag(Tag.Elite)))
            {
                interactable.CreateCancelMainButton(_gameActionsProvider);
                interactable.CreateEffect(creature, new Stat(0, false), RemoveText, PlayActionType.Choose, investigator);

                async Task RemoveText()
                {
                    await _gameActionsProvider.Create(new UpdateStatesGameAction(creature.Blancked, true));
                    CreateOneTimeReaction<GameAction>(RemoveEffectCondition, RemoveEffecLogic, GameActionTime.After);

                    /*******************************************************************/
                    bool RemoveEffectCondition(GameAction gameAction)
                    {
                        if (gameAction != _phase) return false;
                        return true;
                    }

                    async Task RemoveEffecLogic(GameAction gameAction) =>
                        await _gameActionsProvider.Create(new UpdateStatesGameAction(creature.Blancked, false));
                }
            }

            await _gameActionsProvider.Create(interactable);
        }
    }
}
