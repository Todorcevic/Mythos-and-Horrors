using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveInvestigatorToPlaceGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IEnumerable<Investigator> Investigators { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public MoveInvestigatorToPlaceGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigators = new[] { investigator };
            CardPlace = cardPlace;
        }

        public MoveInvestigatorToPlaceGameAction(IEnumerable<Investigator> investigators, CardPlace cardPlace)
        {
            Investigators = investigators;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            IEnumerable<Card> allAvatars = Investigators.Select(investigator => investigator.AvatarCard);
            await _gameActionsProvider.Create(new MoveCardsGameAction(allAvatars, CardPlace.OwnZone));
            await _gameActionsProvider.Create(new CheckRevealPlaceGameAction(CardPlace));
        }
    }
}
