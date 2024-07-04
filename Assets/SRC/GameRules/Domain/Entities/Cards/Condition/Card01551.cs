using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01551 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic };
        public override PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;

        protected IEnumerable<CardCreature> AttackbleCreatures(Investigator investigator) =>
            investigator.CreaturesInSamePlace.Where(creature => creature.InvestigatorAttackTurnsCost.Value <= investigator.CurrentTurns.Value);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            PlayFromHandTurnsCost = CreateStat(0);
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(GameAction gameAction, Investigator investigator)
        {
            InteractableGameAction chooseEnemy = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Enemy");

            foreach (CardCreature creature in AttackbleCreatures(investigator))
            {
                chooseEnemy.CreateEffect(creature, creature.InvestigatorAttackTurnsCost, AttackCreature, PlayActionType.Attack, investigator);

                async Task AttackCreature()
                {
                    AttackCreatureGameAction playAttackGameAction = _gameActionsProvider.Create<AttackCreatureGameAction>()
                        .SetWith(investigator, creature, amountDamage: 3);
                    playAttackGameAction.ChangeStat(investigator.Agility);
                    await playAttackGameAction.Execute();
                }
            }

            await chooseEnemy.Execute();
        }

        protected override bool CanPlayFromHandSpecific(Investigator investigator)
        {
            if (!AttackbleCreatures(investigator).Any()) return false;
            return true;
        }
    }
}
