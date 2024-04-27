using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01535 : CardSupply, IActivable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat ActivateTurnsCost { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Tome };

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

            IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(Owner.CurrentPlace);
            foreach (Investigator investigator in investigators)
            {
                interactableGameAction.Create()
                .SetCard(investigator.AvatarCard)
                .SetInvestigator(investigator)
                .SetLogic(HealthInvestigator);

                /*******************************************************************/
                async Task HealthInvestigator()
                {
                    await _gameActionsProvider.Create(new IncrementStatGameAction(investigator.Health, 1)); //TODO must be a challenge
                };
            }

            await _gameActionsProvider.Create(interactableGameAction);
        }

        public bool ConditionToActivate(Investigator investigator)
        {
            if (!IsInPlay) return false;
            if (Owner != investigator) return false;
            if (ActivateTurnsCost.Value > investigator.CurrentTurns.Value) return false;
            //if (!_investigatorsProvider.AllInvestigatorsInPlay.Any(investigator => investigator.CanBeHealed)) return false;
            return true;
        }
    }
}
