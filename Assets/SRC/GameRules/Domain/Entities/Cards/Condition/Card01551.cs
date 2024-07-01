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
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand | PlayActionType.Attack;

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction chooseEnemy = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Enemy");

            foreach (CardCreature creature in investigator.CreaturesInSamePlace)
            {
                chooseEnemy.CreateEffect(creature, new Stat(0, false), AttackCreature, PlayActionType.Choose, investigator);

                async Task AttackCreature()
                {
                    AttackCreatureGameAction playAttackGameAction = _gameActionsProvider.Create<AttackCreatureGameAction>()
                        .SetWith(investigator, creature, amountDamage: 3);
                    playAttackGameAction.ChangeStat(investigator.Agility);
                    await playAttackGameAction.Start();
                }
            }

            await chooseEnemy.Start();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!ControlOwner.CreaturesInSamePlace.Any()) return false;
            return true;
        }

    }
}
