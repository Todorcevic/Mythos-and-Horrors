using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01121a : CardPlot
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        private Card01121b MaskedHunter => _cardsProvider.GetCard<Card01121b>();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by Injection")]
        private void Init()
        {
            CreateActivation(CreateStat(1), ResignActivate, ResignConditionToActivate, PlayActionType.Resign);
        }

        /*******************************************************************/
        protected override async Task CompleteEffect()
        {
            await _gameActionsProvider.Create(new DrawGameAction(_investigatorProvider.Leader, MaskedHunter));
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
