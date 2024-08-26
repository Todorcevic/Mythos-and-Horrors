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
            _jsonService.SaveFileFromData(_investigatorProvider.AllInvestigators, _filesPath.JSON_SAVE_INVESTIGATORS_PATH);

    }
}
