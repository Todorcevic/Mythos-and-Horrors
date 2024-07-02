using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class FinalizeGameAction : GameAction
    {
        [Inject] private readonly IPresenter<FinalizeGameAction> _finalizePresenter;

        public Resolution Resolution { get; private set; }

        /*******************************************************************/
        public FinalizeGameAction SetWith(Resolution resolution)
        {
            Resolution = resolution;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(Resolution.History).Start();
            await Resolution.Logic.Invoke();
            await _finalizePresenter.PlayAnimationWith(this);
        }
    }
}
