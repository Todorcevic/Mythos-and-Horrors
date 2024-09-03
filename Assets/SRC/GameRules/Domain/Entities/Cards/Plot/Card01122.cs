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
            CreateActivation(1, ResignActivate, ResignConditionToActivate, PlayActionType.Resign, new Localization("Activation_Card01122"));
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create<FinalizeGameAction>().SetWith(_chaptersProvider.CurrentScene.FullResolutions[2]).Execute();
        }

        /*******************************************************************/

        private async Task ResignActivate(Investigator activeInvestigator)
        {
            await _gameActionsProvider.Create<RunAwayGameAction>().SetWith(activeInvestigator).Execute();
        }

        private bool ResignConditionToActivate(Investigator activeInvestigator)
        {
            if (!IsInPlay.IsTrue) return false;
            return true;
        }

    }
}
