using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01135 : CardAdversityLimbo
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Curse };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ExtraStat = CreateStat(0);
        }
        /*******************************************************************/
        protected override async Task ObligationLogic(Investigator investigator)
        {
            InteractableGameAction interactableGameAction = new(canBackToThisInteractable: true, mustShowInCenter: true, "Choose", investigator);
            interactableGameAction.Create().SetCard(this).SetLogic(SpendClue).SetDescription("Spend Clue").SetInvestigator(investigator);
            interactableGameAction.Create().SetCard(this).SetLogic(TakeDamage).SetDescription("Take Fear").SetInvestigator(investigator);
            await _gameActionsProvider.Create(interactableGameAction);

            async Task SpendClue()
            {
                await _gameActionsProvider.Create(new PayHintGameAction(investigator, ExtraStat, 1));
            }

            async Task TakeDamage()
            {
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, this, amountDamage: 2));
            }
        }


    }
}
