using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01177 : CardCreature, ITarget
    {
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        public Investigator TargetInvestigator => _investigatorsProvider.AllInvestigatorsInPlay
            .OrderBy(investigator => investigator.HandSize).First();

        public override IEnumerable<Tag> Tags => new[] { Tag.Monster, Tag.Yithian };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<CreatureAttackGameAction>(DiscardCondition, DiscardLogic, isAtStart: true);
        }

        /*******************************************************************/
        private async Task DiscardLogic(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (creatureAttackGameAction.Investigator.DiscardableCardsInHand.Any())
            {
                InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: false, "Discard", creatureAttackGameAction.Investigator);

                foreach (Card card in creatureAttackGameAction.Investigator.DiscardableCardsInHand)
                {
                    interactableGameAction.CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, creatureAttackGameAction.Investigator);

                    /*******************************************************************/
                    async Task Discard() => await _gameActionsProvider.Create(new DiscardGameAction(card));
                }

                await _gameActionsProvider.Create(interactableGameAction);
            }
            else
            {
                Dictionary<Stat, int> stats = new()
                    {
                        { Damage, 1 },
                        { Fear, 1 }
                    };
                await _gameActionsProvider.Create(new IncrementStatGameAction(stats));

                CreateOneTimeReaction<CreatureAttackGameAction>(RestoreCondition, RestoreLogic, isAtStart: false);

                async Task RestoreLogic(CreatureAttackGameAction creatureAttackGameAction2)
                {
                    Dictionary<Stat, int> stats = new()
                    {
                        { Damage, 1 },
                        { Fear, 1 }
                    };
                    await _gameActionsProvider.Create(new DecrementStatGameAction(stats));
                }

                bool RestoreCondition(CreatureAttackGameAction creatureAttackGameAction2)
                {
                    if (creatureAttackGameAction2 != creatureAttackGameAction) return false;
                    return true;
                }
            }
        }

        private bool DiscardCondition(CreatureAttackGameAction creatureAttackGameAction)
        {
            if (!IsInPlay) return false;
            if (creatureAttackGameAction.Creature != this) return false;
            return true;
        }

        /*******************************************************************/
    }

}
