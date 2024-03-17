using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CheckRevealPlaceGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public CheckRevealPlaceGameAction(CardPlace cardPlace)
        {
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            if (CardPlace.Revealed.IsActive) return;
            await _gameActionsProvider.Create(new RevealGameAction(CardPlace));
        }
    }
}
