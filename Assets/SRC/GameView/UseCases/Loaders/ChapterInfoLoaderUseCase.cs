using MythosAndHorrors.GameRules;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Zenject;

namespace MythosAndHorrors.GameView
{
    public class ChapterInfoLoaderUseCase
    {
        [Inject] private readonly FilesPath _filesPath;
        [Inject] private readonly DataSaveLoaderUseCase _saveDataLoaderUseCase;
        [Inject] private readonly ChaptersProvider chaptersProvider;

        /*******************************************************************/
        public void Execute()
        {
            string jsonData = File.ReadAllText(_filesPath.JSON_CHAPTERINFO_PATH);
            chaptersProvider.AddChapters(JsonConvert.DeserializeObject<List<Chapter>>(jsonData));
            chaptersProvider.SetCurrentDificulty(_saveDataLoaderUseCase.DataSave.DificultySelected);
        }
    }
}
