using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class MoveInvestigatorAndUnconfrontGameAction : MoveInvestigatorToPlaceGameAction
    {
        public MoveInvestigatorAndUnconfrontGameAction(Investigator investigator, CardPlace cardPlace) : base(investigator, cardPlace) { }

        public MoveInvestigatorAndUnconfrontGameAction(IEnumerable<Investigator> investigators, CardPlace cardPlace) : base(investigators, cardPlace) { }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            Dictionary<Card, Zone> allMoves = Investigators.Select(investigator => investigator.AvatarCard).ToDictionary(avatar => (Card)avatar, avatar => CardPlace.OwnZone);

            Investigators.SelectMany(investigator => investigator.BasicCreaturesConfronted)
                .ForEach(creature => allMoves.Add(creature, creature.CurrentPlace.OwnZone));

            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(allMoves).Start();
        }
    }
}
