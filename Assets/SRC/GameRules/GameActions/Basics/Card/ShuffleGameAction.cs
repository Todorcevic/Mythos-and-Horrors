using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShuffleGameAction : GameAction
    {
        private List<Card> _cards;
        [Inject] private readonly IPresenter<ShuffleGameAction> _shufllePresenter;

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
            await _shufllePresenter.PlayAnimationWith(this);
        }

        /*******************************************************************/
        public override async Task Undo()
        {
            ZoneToShuffle.ReorderCardsWith(_cards);
            await base.Undo();
            await _shufllePresenter.PlayAnimationWith(this);
        }
    }
}
