using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class ExhaustCardsGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ExhaustCardsGameAction> _exhaustCardPresenter;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public IEnumerable<Card> Cards { get; private set; }

        /*******************************************************************/
        public ExhaustCardsGameAction(Card card) : this(new[] { card }) { }

        public ExhaustCardsGameAction(IEnumerable<Card> cards)
        {
            Cards = cards;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new UpdateStatesGameAction(Cards.Select(card => card.Exausted), true));
            await _exhaustCardPresenter.PlayAnimationWith(this);
        }
    }
}