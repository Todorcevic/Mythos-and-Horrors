using MythsAndHorrors.GameRules;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AdventurerLoaderUseCase
    {
        [Inject] private readonly JsonService _jsonService;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        /*******************************************************************/
        public void Execute(string adventurerFilePath)
        {
            Adventurer newAdventurer = _jsonService.CreateDataFromFile<Adventurer>(adventurerFilePath);
            _adventurersProvider.AddAdventurer(newAdventurer);
            _avatarViewsManager.GetVoid().Init(newAdventurer);
        }
    }
}
