using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01552 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction chooseEnemy = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, localizableCode: "Interactable_Card01552");

            foreach (CardCreature creature in investigator.CreaturesInSamePlace.Where(creature => creature.Exausted.IsActive))
            {
                chooseEnemy.CreateEffect(creature, new Stat(0, false), DamageCreature, PlayActionType.Choose, investigator, "CardEffect_Card01552");

                /*******************************************************************/
                async Task DamageCreature() => await _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(creature, this, amountDamage: 2).Execute();
            }

            await chooseEnemy.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!ControlOwner.CreaturesInSamePlace.Any(creature => creature.Exausted.IsActive)) return false;
            return true;
        }
    }
}
