using GameRules;
using System.Threading.Tasks;
using Zenject;

namespace GameView
{
    public class ActivatorCardPreseterPresenter : ICardActivator
    {
        [Inject] private readonly CardsManager _cardsManager;

        /*******************************************************************/
        public void ActivateThisCards(params string[] gameActions)
        {
            foreach (string gameAction in gameActions)
            {
                CardView card = _cardsManager.Get(gameAction);
                card.ActivateToSelect();
            }
        }
    }
}
