using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{

    public class ExhaustCardsGameAction : GameAction
    {
        [Inject] private readonly IPresenter<ExhaustCardsGameAction> _exhaustCardPresenter;
        [Inject] private readonly GameActionProvider _gameActionProvider;

        public List<Card> Cards { get; private set; }

        /*******************************************************************/
        public ExhaustCardsGameAction(Card card) : this(new List<Card> { card }) { }

        public ExhaustCardsGameAction(List<Card> cards)
        {
            Cards = cards;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionProvider.Create(new UpdateStatesGameAction(Cards.Select(card => card.Exausted).ToList(), true));
            await _exhaustCardPresenter.PlayAnimationWith(this);
        }
    }
}