using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MoveInvestigatorGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public List<Investigator> Investigators { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public MoveInvestigatorGameAction(List<Investigator> investigators, CardPlace cardPlace)
        {
            Investigators = investigators;
            CardPlace = cardPlace;
        }

        public MoveInvestigatorGameAction(Investigator investigator, CardPlace cardPlace) :
            this(new List<Investigator> { investigator }, cardPlace)
        { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            List<Card> allAvatarCards = Investigators.Select(investigator => investigator.AvatarCard as Card).ToList();
            await _gameActionFactory.Create(new MoveCardsGameAction(allAvatarCards, CardPlace.OwnZone));
        }
    }
}
