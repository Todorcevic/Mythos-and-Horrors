using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardWeapon : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ChooseCreatureLogic, AttackCondition, PlayActionType.Activate | PlayActionType.Attack, new Localization("Activation_CardWeapon"));
        }

        /*******************************************************************/
        protected virtual bool AttackCondition(Investigator investigator)
        {
            if (!IsInPlay.IsTrue) return false;
            if (investigator != ControlOwner) return false;
            if (!investigator.CreaturesInSamePlace.Any()) return false;
            return true;
        }

        protected async Task ChooseCreatureLogic(Investigator investigator)
        {
            InteractableGameAction chooseEnemy = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_CardWeapon"));

            foreach (CardCreature creature in investigator.CreaturesInSamePlace)
            {
                chooseEnemy.CreateCardEffect(creature, new Stat(0, false), AttackCreature, PlayActionType.Choose,
                    investigator, new Localization("CardEffect_CardWeapon", Info.Name));

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
