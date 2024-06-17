using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01551 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        protected override bool IsFast => false;
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Attack;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            InteractableGameAction chooseEnemy = new(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Enemy");

            chooseEnemy.CreateCancelMainButton();

            foreach (CardCreature creature in investigator.CreaturesInSamePlace)
            {
                chooseEnemy.CreateEffect(creature, new Stat(0, false), AttackCreature, PlayActionType.Choose, investigator);

                async Task AttackCreature()
                {
                    AttackCreatureGameAction playAttackGameAction = new(investigator, creature, amountDamage: 3);
                    playAttackGameAction.ChangeStat(investigator.Agility);
                    await _gameActionsProvider.Create(playAttackGameAction);
                }
            }

            await _gameActionsProvider.Create(chooseEnemy);
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction)
        {
            if (!ControlOwner.CreaturesInSamePlace.Any()) return false;
            return true;
        }

    }
}
