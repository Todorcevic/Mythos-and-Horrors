using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SaveGameUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly AdventurerProvider _adventurerRepository;

        public void Execute()
        {
            IReadOnlyList<Adventurer> allAdventurers = _adventurerRepository.GetAllAdventurers();
            _jsonService.SaveFileFromData(allAdventurers, FilesPath.JSON_ADVENTURERS_PATH);
        }
    }
}
