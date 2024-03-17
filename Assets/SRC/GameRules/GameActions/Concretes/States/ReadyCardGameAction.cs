using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReadyCardGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ReadyCardGameAction> _readyCardPresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IEnumerable<Card> Cards { get; }

        /*******************************************************************/
        public ReadyCardGameAction(Card card) : this(new[] { card }) { }

        public ReadyCardGameAction(IEnumerable<Card> cards)
        {
            Cards = cards;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Cards.Select(card => card.Exausted), false));
            await _readyCardPresenter.PlayAnimationWith(this);
        }
    }
}