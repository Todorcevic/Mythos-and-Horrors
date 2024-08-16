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
        protected override string LocalizableCode => "OptativeReaction_Card01568";

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
            InteractableGameAction interactable = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01568");
            foreach (CardCreature creature in investigator.CreaturesInSamePlace.Where(creature => !creature.HasThisTag(Tag.Elite)))
            {
                interactable.CreateCardEffect(creature, new Stat(0, false), RemoveText, PlayActionType.Choose, investigator, "CardEffect_Card01568");
                async Task RemoveText()
                {
                    await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(creature.Blancked, true).Execute();
                    CreateOneTimeReaction<GameAction>(RemoveEffectCondition, RemoveEffecLogic, GameActionTime.After);

                    /*******************************************************************/
                    bool RemoveEffectCondition(GameAction gameAction)
                    {
                        if (gameAction != _phase) return false;
                        return true;
                    }

                    async Task RemoveEffecLogic(GameAction gameAction) =>
                        await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(creature.Blancked, false).Execute();
                }
            }

            await interactable.Execute();
        }
    }
}
