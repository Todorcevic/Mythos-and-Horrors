using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01122 : CardPlot
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ResignActivate, ResignConditionToActivate, withOpportunityAttck: false);
        }

        /*******************************************************************/
        public override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new FinalizeGameAction(_chaptersProvider.CurrentScene.FullResolutions[2]));
        }

        /*******************************************************************/

        private async Task ResignActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create(new ResignGameAction(activeInvestigator));
        }

        private bool ResignConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay) return false;
            return true;
        }

    }
}
