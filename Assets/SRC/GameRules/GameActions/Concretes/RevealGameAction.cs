using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RevealGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Card Card { get; }

        /*******************************************************************/
        public RevealGameAction(Card cardReveled)
        {
            Card = cardReveled;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}
