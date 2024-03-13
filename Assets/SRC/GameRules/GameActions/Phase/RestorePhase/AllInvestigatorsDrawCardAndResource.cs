using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    //4.4	Each investigator draws 1 card and gains 1 resource.
    public class AllInvestigatorsDrawCardAndResource : PhaseGameAction
    {
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Name) + nameof(AllInvestigatorsDrawCardAndResource);
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Description) + nameof(AllInvestigatorsDrawCardAndResource);
        public override Phase MainPhase => Phase.Restore;

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.AllInvestigators.FindAll(i => i.HandSize > 0)) //TODO: Quitar
            {
                await _gameActionProvider.Create(new DrawGameAction(investigator));
                await _gameActionProvider.Create(new GainResourceGameAction(investigator, 1));
            }
        }
    }
}
