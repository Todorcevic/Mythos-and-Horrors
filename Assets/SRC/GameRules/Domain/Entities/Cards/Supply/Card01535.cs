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
            CreateActivation(CreateStat(1), HealthActivate, HealtConditionConditionToActivate);
        }

        /************************ HEALTH ACTIVATION ******************************/
        public async Task HealthActivate(Investigator activeInvestigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Health");
            interactableGameAction.CreateCancelMainButton();

            IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(activeInvestigator.CurrentPlace);
            foreach (Investigator investigatorToSelect in investigators)
            {
                interactableGameAction.Create()
                .SetCard(investigatorToSelect.AvatarCard)
                .SetInvestigator(investigatorToSelect)
                .SetLogic(HealthInvestigator);

                /*******************************************************************/
                async Task HealthInvestigator()
                {
                    await _gameActionsProvider.Create(new IncrementStatGameAction(investigatorToSelect.Health, 1)); //TODO must be a challenge
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }

        public bool HealtConditionConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            if (Owner != activeInvestigator) return false;
            return true;
        }
    }
}
