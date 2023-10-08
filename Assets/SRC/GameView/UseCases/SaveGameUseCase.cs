using MythsAndHorrors.GameRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class SaveGameUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly AdventurerRepository _adventurerRepository;

        public void Execute()
        {
            IReadOnlyList<Adventurer> allAdventurers = _adventurerRepository.GetAllAdventurers();
            _jsonService.SaveFileFromData(allAdventurers, FilesPath.JSON_ADVENTURERS_PATH);
        }
    }
}
