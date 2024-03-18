using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ShuffleGameAction : GameAction
    {
        private readonly List<Card> _cards;
        [Inject] private readonly IPresenter<ShuffleGameAction> _shufllePresenter;

        public Zone ZoneToShuffle { get; }

        /*******************************************************************/
        public ShuffleGameAction(Zone zoneToShuffle)
        {
            ZoneToShuffle = zoneToShuffle;
            _cards = ZoneToShuffle.Cards.ToList();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            ZoneToShuffle.Shuffle();
            await _shufllePresenter.PlayAnimationWith(this);
        }

        public override async Task Undo()
        {
            ZoneToShuffle.ReorderCardsWith(_cards);
            await _shufllePresenter.PlayAnimationWith(this);
        }
    }
}
