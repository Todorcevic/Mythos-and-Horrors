using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : PhaseGameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly IPresenter<ChooseInvestigatorGameAction> _startingAnimationPresenter;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStart)
            {
                investigator.AvatarCard.AddEffect(investigator, _textsProvider.GameText.DEFAULT_VOID_TEXT, ChooseInvestigatorEffect, withReturn: true);

                async Task ChooseInvestigatorEffect()
                {
                    ActiveInvestigator = investigator;
                    await _startingAnimationPresenter.PlayAnimationWith(this);
                };
            }

            await _gameActionFactory.Create(new InteractableGameAction(isMandatary: true));
        }
    }
}
