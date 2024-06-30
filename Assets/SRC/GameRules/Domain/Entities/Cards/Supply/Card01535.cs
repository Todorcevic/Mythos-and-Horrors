using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01535 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tome, Tag.Item };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, HealthActivate, HealtConditionConditionToActivate, PlayActionType.Activate);
        }

        /************************ HEALTH ACTIVATION ******************************/
        public async Task HealthActivate(Investigator activeInvestigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Health");
            interactableGameAction.CreateCancelMainButton();

            IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(activeInvestigator.CurrentPlace);
            foreach (Investigator investigatorToSelect in investigators)
            {
                interactableGameAction.CreateEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false), HealthInvestigator, PlayActionType.Choose, activeInvestigator);

                /*******************************************************************/
                async Task HealthInvestigator()
                {
                    await _gameActionsProvider.Create(new ChallengePhaseGameAction(activeInvestigator.Intelligence, 2, "Health", this, succesEffect: HealthInvestigator, failEffect: DamageInvestigator));

                    /*******************************************************************/
                    async Task HealthInvestigator() => await _gameActionsProvider.Create(new HealthGameAction(investigatorToSelect.InvestigatorCard, amountDamageToRecovery: 1));
                    async Task DamageInvestigator() => await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigatorToSelect, this, amountDamage: 1));
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }

        public bool HealtConditionConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != activeInvestigator) return false;
            return true;
        }
    }
}
