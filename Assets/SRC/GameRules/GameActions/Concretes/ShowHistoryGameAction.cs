using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;

        public History History { get; }

        /*******************************************************************/
        public ShowHistoryGameAction(History history)
        {
            History = history;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _animator.PlayAnimationWith(this);
        }
    }
}
