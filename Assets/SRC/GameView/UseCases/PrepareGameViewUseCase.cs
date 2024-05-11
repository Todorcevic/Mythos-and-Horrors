using Zenject;

namespace MythosAndHorrors.GameView
{
    public class PrepareGameViewUseCase
    {
        [Inject] private readonly TextsLoaderUseCase _textsLoaderUseCase;
        [Inject] private readonly AvatarViewLoaderUseCase _avatarLoaderUseCase;
        [Inject] private readonly ZoneViewLoaderUseCase _zoneLoaderUseCase;
        [Inject] private readonly CardViewGeneratorComponent _cardGeneratorComponent;

        /*******************************************************************/
        public void Execute()
        {
            _textsLoaderUseCase.LoadGameTexts();
            _textsLoaderUseCase.LoadViewTexts();
            _avatarLoaderUseCase.Execute();
            _zoneLoaderUseCase.Execute();
            _cardGeneratorComponent.BuildAllCardViews();
        }
    }
}
