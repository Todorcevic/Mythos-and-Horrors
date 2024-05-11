using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PrepareAllUseCase
    {
        [Inject] private readonly PrepareGameRulesUseCase _loadGameRulesUseCase;
        [Inject] private readonly PrepareGameViewUseCase _loadGameViewUseCase;

        /*******************************************************************/
        public void Execute()
        {
            _loadGameRulesUseCase.Execute();
            _loadGameViewUseCase.Execute();
        }
    }
}
