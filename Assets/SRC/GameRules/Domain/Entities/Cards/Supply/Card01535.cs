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
            CreateActivation(1, HealthActivate, HealtConditionConditionToActivate, PlayActionType.Activate, "Activation_Card01535");
        }

        /************************ HEALTH ACTIVATION ******************************/
        public async Task HealthActivate(Investigator activeInvestigator)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01535");

            IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(activeInvestigator.CurrentPlace);
            foreach (Investigator investigatorToSelect in investigators)
            {
                interactableGameAction.CreateEffect(investigatorToSelect.InvestigatorCard, new Stat(0, false), HealthInvestigator,
                    PlayActionType.Choose, activeInvestigator, "CardEffect_Card01535");

                /*******************************************************************/
                async Task HealthInvestigator()
                {
                    await _gameActionsProvider.Create<ChallengePhaseGameAction>()
                        .SetWith(activeInvestigator.Intelligence, 2, "Health", this, succesEffect: HealthInvestigator, failEffect: DamageInvestigator).Execute();

                    /*******************************************************************/
                    async Task HealthInvestigator() => await _gameActionsProvider.Create<RecoverGameAction>().SetWith(investigatorToSelect.InvestigatorCard, amountDamageToRecovery: 1).Execute();
                    async Task DamageInvestigator() => await _gameActionsProvider.Create<HarmToInvestigatorGameAction>().SetWith(investigatorToSelect, this, amountDamage: 1).Execute();
                };
            }

            await interactableGameAction.Execute();
        }

        public bool HealtConditionConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (ControlOwner != activeInvestigator) return false;
            return true;
        }
    }
}
