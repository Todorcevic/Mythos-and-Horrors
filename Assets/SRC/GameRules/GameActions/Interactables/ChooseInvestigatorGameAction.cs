using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : GameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;
        [Inject] private readonly IPresenter<ChooseInvestigatorGameAction> _startingAnimationPresenter;

        public List<Investigator> InvestigatorsToSelect { get; }
        public Investigator InvestigatorSelected { get; private set; }

        /*******************************************************************/
        public ChooseInvestigatorGameAction(List<Investigator> investigatorsToSelect)
        {
            InvestigatorsToSelect = investigatorsToSelect;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            foreach (Investigator investigator in InvestigatorsToSelect)
            {
                _effectProvider.Create()
                    .SetCard(investigator.AvatarCard)
                    .SetInvestigator(investigator)
                    .SetCanPlay(() => true)
                    .SetDescription(_textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(ChooseInvestigator))
                    .SetLogic(ChooseInvestigator);

                /*******************************************************************/
                async Task ChooseInvestigator()
                {
                    InvestigatorSelected = investigator;
                    await _startingAnimationPresenter.PlayAnimationWith(this);
                };
            }

            await _gameActionProvider.Create(new InteractableGameAction());
        }
    }
}
