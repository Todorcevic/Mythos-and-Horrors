using MythsAndHorrors.EditMode;
using Zenject;

namespace MythsAndHorrors.PlayMode
{
    public class AdventurerLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly AdventurersProvider _adventurersProvider;

        /*******************************************************************/
        public void Execute(string adventurerFilePath)
        {
            Adventurer newAdvewnture = _jsonService.CreateDataFromFile<Adventurer>(adventurerFilePath);
            _adventurersProvider.AddAdventurer(newAdvewnture);
        }
    }
}
