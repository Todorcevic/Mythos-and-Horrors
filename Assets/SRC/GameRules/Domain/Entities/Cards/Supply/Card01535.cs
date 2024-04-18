using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01535 : CardSupply, ITome, IActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat ActivateTurnsCost { get; private set; }

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ActivateTurnsCost = new Stat(1);
        }

        /************************ HEALTH ACTIVATION ******************************/
        public async Task Activate()
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: false, mustShowInCenter: true, "Health");
            interactableGameAction.CreateUndoButton().SetLogic(UndoEffect);
            async Task UndoEffect()
            {
                await Task.CompletedTask;
            }

            IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace)
                .Where(investigator => investigator.CanBeHealed);
            foreach (Investigator investigator in investigators)
            {
                interactableGameAction.Create()
                .SetCard(investigator.AvatarCard)
                .SetInvestigator(investigator)
                .SetLogic(HealthInvestigator);

                /*******************************************************************/
                async Task HealthInvestigator()
                {
                    await _gameActionsProvider.Create(new IncrementStatGameAction(investigator.Health, 1));
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }
    }
}
