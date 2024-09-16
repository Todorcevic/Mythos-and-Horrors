using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class ShuffleGameAction : GameAction
    {
        private List<Card> _cards;

        public Zone ZoneToShuffle { get; private set; }
        public override bool CanUndo => false;

        /*******************************************************************/
        public ShuffleGameAction SetWith(Zone zoneToShuffle)
        {
            ZoneToShuffle = zoneToShuffle;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            _cards = ZoneToShuffle.Cards.ToList();
            ZoneToShuffle.Shuffle();
            await Task.CompletedTask;
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            ZoneToShuffle.ReorderCardsWith(_cards);
            await base.Undo();
        }
    }
}
