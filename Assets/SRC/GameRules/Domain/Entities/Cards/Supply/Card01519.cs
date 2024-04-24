using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01519 : CardSupply, IActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public int InitialSupplies => 3;
        public Stat ActivateTurnsCost { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            AmountSupplies = new Stat(InitialSupplies);
            ActivateTurnsCost = new Stat(1);
        }

        /*******************************************************************/
        public async Task Activate()
        {
            await _gameActionsProvider.Create(new DecrementStatGameAction(AmountSupplies, 1));

            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Select Investigator");
            interactableGameAction.CreateMainButton().SetLogic(CancelEffect);

            async Task CancelEffect()
            {
                await _gameActionsProvider.UndoLastInteractable();
            }

            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace)
                .Where(investigator => investigator.CanBeHealed))
            {
                interactableGameAction.Create()
                    .SetCard(investigator.AvatarCard)
                    .SetInvestigator(Owner)
                    .SetCardAffected(investigator.InvestigatorCard)
                    .SetLogic(RestoreHealthInvestigator);

                /*******************************************************************/
                async Task RestoreHealthInvestigator()
                {
                    await _gameActionsProvider.Create(new IncrementStatGameAction(investigator.Health, 1));
                };
            }

            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace)
                .Where(investigator => investigator.CanBeRestoreSanity))
            {
                interactableGameAction.Create()
                    .SetCard(investigator.AvatarCard)
                    .SetInvestigator(Owner)
                    .SetCardAffected(investigator.InvestigatorCard)
                    .SetLogic(RestoreSanityInvestigator);

                /*******************************************************************/
                async Task RestoreSanityInvestigator()
                {
                    await _gameActionsProvider.Create(new IncrementStatGameAction(investigator.Sanity, 1));
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
