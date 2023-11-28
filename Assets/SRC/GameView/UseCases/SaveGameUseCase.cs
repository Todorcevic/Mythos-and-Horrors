using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SaveGameUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly AdventurersProvider _adventurerRepository;
        [Inject] private readonly FilesPath _filesPath;

        /*******************************************************************/
        public void Execute() =>
            _jsonService.SaveFileFromData(_adventurerRepository.AllAdventurers, _filesPath.JSON_ADVENTURERS_PATH);

    }
}
