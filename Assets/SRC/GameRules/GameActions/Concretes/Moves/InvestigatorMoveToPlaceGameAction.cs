using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigatorMoveToPlaceGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public Investigator Investigator { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public InvestigatorMoveToPlaceGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigator = investigator;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new MoveCardsGameAction(Investigator.AvatarCard, CardPlace.OwnZone));
        }
    }
}
