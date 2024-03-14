using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ReadyCardGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ReadyCardGameAction> _readyCardPresenter;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public List<Card> Cards { get; private set; }

        /*******************************************************************/
        public ReadyCardGameAction(Card card) : this(new List<Card> { card }) { }

        public ReadyCardGameAction(List<Card> cards)
        {
            Cards = cards;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionProvider.Create(new UpdateStatesGameAction(Cards.Select(card => card.Exausted).ToList(), false));
            await _readyCardPresenter.PlayAnimationWith(this);
        }
    }
}