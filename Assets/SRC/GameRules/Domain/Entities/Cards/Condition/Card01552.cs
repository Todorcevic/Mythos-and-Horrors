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
        protected override bool IsFast => false;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            InteractableGameAction chooseEnemy = new(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Enemy");
            chooseEnemy.CreateCancelMainButton();

            foreach (CardCreature creature in investigator.CreaturesInSamePlace.Where(creature => creature.Exausted.IsActive))
            {
                chooseEnemy.CreateEffect(creature, new Stat(0, false), DamageCreature, PlayActionType.Choose, investigator);

                async Task DamageCreature() => await _gameActionsProvider.Create(new HarmToCardGameAction(creature, this, amountDamage: 2));
            }

            await _gameActionsProvider.Create(chooseEnemy);
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (!ControlOwner.CreaturesInSamePlace.Any(creature => creature.Exausted.IsActive)) return false;
            return true;
        }

    }
}
