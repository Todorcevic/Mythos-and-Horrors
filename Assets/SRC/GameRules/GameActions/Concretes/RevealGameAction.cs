using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RevealGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public IRevellable RevellableCard { get; init; }

        /*******************************************************************/
        public RevealGameAction(IRevellable cardReveled)
        {
            RevellableCard = cardReveled;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            RevellableCard.Reveal();
            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}
