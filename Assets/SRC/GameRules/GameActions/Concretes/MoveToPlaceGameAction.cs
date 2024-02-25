using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveToPlaceGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public MoveToPlaceGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigator = investigator;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionFactory.Create(new DecrementStatGameAction(Investigator.Turns, CardPlace.MoveCost.Value));
            await _gameActionFactory.Create(new MoveCardsGameAction(Investigator.AvatarCard, CardPlace.OwnZone));
        }
    }
}
