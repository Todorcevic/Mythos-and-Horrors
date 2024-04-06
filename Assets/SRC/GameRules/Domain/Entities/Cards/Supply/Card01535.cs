using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01535 : CardSupply
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;

        public Stat HealthActivationTurnsCost { get; private set; }
        public Effect ActivateEffect { get; private set; }

        /*******************************************************************/
        [Inject]
        private void Init()
        {
            HealthActivationTurnsCost = new Stat(1);
        }

        /*******************************************************************/
        protected override async Task WhenBegin(GameAction gameAction)
        {
            await base.WhenBegin(gameAction);
            CheckHealthActivation(gameAction);
        }

        /************************ HEALTH ACTIVATION ******************************/
        private void CheckHealthActivation(GameAction gameAction)
        {
            if (gameAction is not InteractableGameAction interactableGameAction) return;
            if (interactableGameAction.Parent is not OneInvestigatorTurnGameAction oneInvestigatorTurnGA) return;
            if (CurrentZone != oneInvestigatorTurnGA.ActiveInvestigator.AidZone) return;
            if (oneInvestigatorTurnGA.ActiveInvestigator.CurrentTurns.Value < HealthActivationTurnsCost.Value) return;

            interactableGameAction.Create()
                .SetCard(this)
                .SetInvestigator(oneInvestigatorTurnGA.ActiveInvestigator)
                .SetLogic(HealthActivation)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(HealthActivation));

            async Task HealthActivation()
            {
                IEnumerable<Investigator> investigators = _investigatorsProvider.GetInvestigatorsInThisPlace(oneInvestigatorTurnGA.ActiveInvestigator.CurrentPlace);
                ChooseInvestigatorGameAction chooseInvestigatorGA = await _gameActionsProvider.Create(new ChooseInvestigatorGameAction(investigators));
                if (!chooseInvestigatorGA.InvestigatorSelected.CanBeHealed) return;
                await _gameActionsProvider.Create(new IncrementStatGameAction(chooseInvestigatorGA.InvestigatorSelected.Health, 1));
            }
        }
    }
}
