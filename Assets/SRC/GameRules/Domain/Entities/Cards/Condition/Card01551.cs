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
            InteractableGameAction chooseCreature = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01551"));

            foreach (CardCreature creature in investigator.CreaturesInSamePlace)
            {
                chooseCreature.CreateCardEffect(creature, new Stat(0, false), AttackCreature, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01551"));

                async Task AttackCreature()
                {
                    AttackCreatureGameAction playAttackGameAction = _gameActionsProvider.Create<AttackCreatureGameAction>()
                        .SetWith(investigator, creature, amountDamage: investigator.BasicDamegeToAttack.Value + 2);
                    playAttackGameAction.ChangeStat(investigator.Agility);
                    await playAttackGameAction.Execute();
                }
            }

            await chooseCreature.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }
    }
}
