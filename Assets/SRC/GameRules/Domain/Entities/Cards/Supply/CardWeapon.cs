using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CardWeapon : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ChooseEnemyLogic, AttackCondition, withOpportunityAttck: false);
        }

        /*******************************************************************/
        public virtual bool AttackCondition(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }

        private async Task ChooseEnemyLogic(Investigator investigator)
        {
            InteractableGameAction chooseEnemy = new(canBackToThisInteractable: false, mustShowInCenter: true,
                description: "Choose Enemy", activeInvestigator: investigator);

            foreach (CardCreature creature in investigator.CreaturesInSamePlace)
            {
                chooseEnemy.Create().SetCard(creature).SetDescription("Choose " + creature.Info.Name).SetLogic(() => AttackEnemy(creature));
            }

            await _gameActionsProvider.Create(chooseEnemy);
        }

        public virtual async Task AttackEnemy(CardCreature creature) =>
            await _gameActionsProvider.Create(new AttackGameAction(ControlOwner, creature, 2));

    }
}
