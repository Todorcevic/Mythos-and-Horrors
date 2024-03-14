using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveInvestigatorToPlaceGameAction : GameAction
    {
        [Inject] private readonly GameActionProvider _gameActionFactory;

        public List<Investigator> Investigators { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public MoveInvestigatorToPlaceGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigators = new() { investigator };
            CardPlace = cardPlace;
        }

        public MoveInvestigatorToPlaceGameAction(List<Investigator> investigators, CardPlace cardPlace)
        {
            Investigators = investigators;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            List<Card> allAvatars = Investigators.Select(investigator => investigator.AvatarCard).Cast<Card>().ToList();
            await _gameActionFactory.Create(new MoveCardsGameAction(allAvatars, CardPlace.OwnZone));
            await _gameActionFactory.Create(new CheckRevealPlaceGameAction(CardPlace));
        }
    }
}
