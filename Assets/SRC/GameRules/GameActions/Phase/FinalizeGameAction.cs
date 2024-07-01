using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class FinalizeGameAction : GameAction
    {
        [Inject] private readonly IPresenter<FinalizeGameAction> _finalizePresenter;

        public Resolution Resolution { get; }

        /*******************************************************************/
        public FinalizeGameAction(Resolution resolution)
        {
            Resolution = resolution;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new ShowHistoryGameAction(Resolution.History));
            await Resolution.Logic.Invoke();
            await _finalizePresenter.PlayAnimationWith(this);
        }
    }
}
