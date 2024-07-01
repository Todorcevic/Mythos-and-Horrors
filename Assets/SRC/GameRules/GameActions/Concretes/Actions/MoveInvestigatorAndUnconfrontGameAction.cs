using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class MoveInvestigatorAndUnconfrontGameAction : MoveInvestigatorToPlaceGameAction
    {
        public new MoveInvestigatorAndUnconfrontGameAction SetWith(Investigator investigator, CardPlace cardPlace)
        {
            base.SetWith(investigator, cardPlace);
            return this;
        }

        public new MoveInvestigatorAndUnconfrontGameAction SetWith(IEnumerable<Investigator> investigators, CardPlace cardPlace)
        {
            base.SetWith(investigators, cardPlace);
            return this;
        }

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
