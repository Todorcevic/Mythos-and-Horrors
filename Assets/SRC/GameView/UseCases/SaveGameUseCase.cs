using MythosAndHorrors.GameRules;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class SaveGameUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly InvestigatorsProvider _investigatorProvider;
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        public void Execute() =>
            _jsonService.SaveFileFromData(_investigatorProvider.Investigators, _filesPath.JSON_INVESTIGATORS_PATH);

    }
}
