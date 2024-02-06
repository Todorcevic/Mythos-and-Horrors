using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayersProvider;

        public History History { get; }

        /*******************************************************************/
        public ShowHistoryGameAction(History history)
        {
            History = history;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _viewLayersProvider.PlayAnimationWith(this);
        }
    }
}
