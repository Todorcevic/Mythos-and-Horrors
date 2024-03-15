using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class Card01535 : CardSupply
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly EffectsProvider _effectProvider;
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
        protected void CheckHealthActivation(GameAction gameAction)
        {
            if (gameAction is not OneInvestigatorTurnGameAction oneTurnGA) return;
            if (!CanHealthActivation()) return;

            _effectProvider.Create()
                .SetCard(this)
                .SetInvestigator(_investigatorProvider.ActiveInvestigator)
                .SetLogic(HealthActivation)
                .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(HealthActivation));
        }

        protected bool CanHealthActivation()
        {
            if (_investigatorProvider.ActiveInvestigator.AidZone != CurrentZone) return false;
            if (_investigatorProvider.ActiveInvestigator.CurrentTurns.Value < HealthActivationTurnsCost.Value) return false;
            return true;
        }

        protected async Task HealthActivation()
        {
            ChooseInvestigatorGameAction chooseInvestigatorGA =
                    await _gameActionFactory.Create(new ChooseInvestigatorGameAction(_investigatorsProvider.GetInvestigatorsInThisPlace(_investigatorProvider.ActiveInvestigator.CurrentPlace)));
            if (!chooseInvestigatorGA.InvestigatorSelected.CanBeHealed) return;
            await _gameActionFactory.Create(new IncrementStatGameAction(chooseInvestigatorGA.InvestigatorSelected.Health, 1));
        }
    }
}
