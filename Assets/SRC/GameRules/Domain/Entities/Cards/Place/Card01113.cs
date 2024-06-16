using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01113 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateReaction<MoveInvestigatorToPlaceGameAction>(TakeFearCondition, TakeFearLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task TakeFearLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(() => moveInvestigatorToPlaceGameAction.Investigators
            .Where(investigator => investigator.CurrentPlace == this), TekeFear));

            async Task TekeFear(Investigator investigator) =>
                await _gameActionsProvider.Create(new HarmToInvestigatorGameAction(investigator, amountFear: 1, fromCard: this));
        }

        private bool TakeFearCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }
    }
}
