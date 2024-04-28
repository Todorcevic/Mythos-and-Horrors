using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01113 : CardPlace
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        /*******************************************************************/
        protected override async Task WhenFinish(GameAction gameAction)
        {
            await base.WhenFinish(gameAction);
            await Reaction<MoveInvestigatorToPlaceGameAction>(gameAction, TakeFearCondition, TakeFearLogic);
        }

        /*******************************************************************/
        private async Task TakeFearLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            await _gameActionsProvider.Create(new SafeForeach<Investigator>(moveInvestigatorToPlaceGameAction.Investigators, TekeFear));

            async Task TekeFear(Investigator investigator) =>
                await _gameActionsProvider.Create(new ShareDamageAndFearGameAction(investigator, amountFear: 1, bythisCard: this));
        }

        private bool TakeFearCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorToPlaceGameAction)
        {
            if (moveInvestigatorToPlaceGameAction.CardPlace != this) return false;
            return true;
        }

    }
}
