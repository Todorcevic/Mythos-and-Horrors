using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveInvestigatorToPlaceGameAction : GameAction
    {
        public IEnumerable<Investigator> Investigators { get; }
        public Dictionary<Investigator, CardPlace> From { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public MoveInvestigatorToPlaceGameAction(Investigator investigator, CardPlace cardPlace)
        {
            From = new Dictionary<Investigator, CardPlace> { { investigator, investigator.CurrentPlace } };
            Investigators = new[] { investigator };
            CardPlace = cardPlace;
        }

        public MoveInvestigatorToPlaceGameAction(IEnumerable<Investigator> investigators, CardPlace cardPlace)
        {
            From = investigators.ToDictionary(investigator => investigator, investigator => investigator.CurrentPlace);
            Investigators = investigators;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            IEnumerable<Card> allAvatars = Investigators.Select(investigator => investigator.AvatarCard);
            await _gameActionsProvider.Create(new MoveCardsGameAction(allAvatars, CardPlace.OwnZone));
        }
    }
}
