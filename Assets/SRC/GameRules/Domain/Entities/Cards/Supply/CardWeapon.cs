using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardWeapon : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        protected IEnumerable<CardCreature> AttackbleCreatures =>
            ControlOwner?.CreaturesInSamePlace.Where(creature => creature.InvestigatorAttackTurnsCost.Value <= ControlOwner.CurrentTurns.Value);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(0), ChooseEnemyLogic, AttackCondition, withOpportunityAttck: false);
        }

        /*******************************************************************/
        protected virtual bool AttackCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (!AttackbleCreatures.Any()) return false;
            return true;
        }

        private async Task ChooseEnemyLogic(Investigator investigator)
        {
            InteractableGameAction chooseEnemy = new(canBackToThisInteractable: false, mustShowInCenter: true,
                description: "Choose Enemy", activeInvestigator: investigator);

            foreach (CardCreature creature in AttackbleCreatures)
            {
                chooseEnemy.Create(creature, () => AttackEnemy(creature), PlayActionType.None, investigator);
            }

            await _gameActionsProvider.Create(chooseEnemy);
        }

        protected virtual async Task AttackEnemy(CardCreature creature) =>
            await _gameActionsProvider.Create(new PlayAttackGameAction(ControlOwner, creature, 2));

    }
}
