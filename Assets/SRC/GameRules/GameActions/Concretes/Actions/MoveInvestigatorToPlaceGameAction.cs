using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MoveInvestigatorToPlaceGameAction : GameAction
    {
        public IEnumerable<Investigator> Investigators { get; private set; }
        public Dictionary<Investigator, CardPlace> From { get; private set; }
        public CardPlace CardPlace { get; private set; }

        /*******************************************************************/
        public MoveInvestigatorToPlaceGameAction SetWith(Investigator investigator, CardPlace cardPlace)
        {
            From = new Dictionary<Investigator, CardPlace> { { investigator, investigator.CurrentPlace } };
            Investigators = new[] { investigator };
            CardPlace = cardPlace;
            return this;
        }

        public MoveInvestigatorToPlaceGameAction SetWith(IEnumerable<Investigator> investigators, CardPlace cardPlace)
        {
            From = investigators.ToDictionary(investigator => investigator, investigator => investigator.CurrentPlace);
            Investigators = investigators;
            CardPlace = cardPlace;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            IEnumerable<Card> allAvatars = Investigators.Select(investigator => investigator.AvatarCard);
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(allAvatars, CardPlace.OwnZone).Execute();
        }
    }
}
