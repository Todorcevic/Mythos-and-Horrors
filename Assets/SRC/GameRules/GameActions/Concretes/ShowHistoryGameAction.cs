using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class ShowHistoryGameAction : GameAction
    {
        [Inject] private readonly IViewLayer _animator;
        public History History { get; private set; }

        /*******************************************************************/
        public async Task Run(History history)
        {
            History = history;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _animator.PlayAnimationWith(this);
        }
    }
}
