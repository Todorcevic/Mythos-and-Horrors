using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RevealPlaceGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public CardPlace Card { get; init; }

        /*******************************************************************/
        public RevealPlaceGameAction(CardPlace cardPlaceReveled)
        {
            Card = cardPlaceReveled;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}
