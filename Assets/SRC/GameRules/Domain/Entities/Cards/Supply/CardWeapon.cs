using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardWeapon : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        protected IEnumerable<CardCreature> AttackbleCreatures =>
            ControlOwner?.CreaturesInSamePlace.Where(creature => creature.InvestigatorAttackTurnsCost.Value <= ControlOwner.CurrentTurns.Value);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ChooseEnemyLogic, AttackCondition, PlayActionType.Attack);
        }

        /*******************************************************************/
        protected virtual bool AttackCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (!AttackbleCreatures.Any()) return false;
            return true;
        }

        protected async Task ChooseEnemyLogic(Investigator investigator)
        {
            InteractableGameAction chooseEnemy = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, description: "Choose Enemy");

            foreach (CardCreature creature in AttackbleCreatures)
            {
                chooseEnemy.CreateEffect(creature, new Stat(0, false), AttackCreature, PlayActionType.Choose, investigator);

                /*******************************************************************/
                async Task AttackCreature()
                {
                    AttackCreatureGameAction attackCreatureGameAction = _gameActionsProvider.Create<AttackCreatureGameAction>()
                        .SetWith(investigator, creature, amountDamage: 1);
                    await ExtraAttackEnemyLogic(attackCreatureGameAction);
                    await attackCreatureGameAction.Execute();
                }
            }

            await chooseEnemy.Execute();
        }

        protected abstract Task ExtraAttackEnemyLogic(AttackCreatureGameAction attackCreatureGameAction);
    }
}
