using MythsAndHorrors.GameRules;
using System.Collections.Generic;
using System.IO;
using Unity.Plastic.Newtonsoft.Json;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class ChapterInfoLoaderUseCase
    {
        [Inject] private readonly ChaptersProvider chaptersProvider;

        /*******************************************************************/
        public void Execute(string path, Dificulty dificulty)
        {
            string jsonData = File.ReadAllText(path);
            chaptersProvider.AddChapters(JsonConvert.DeserializeObject<List<ChapterInfo>>(jsonData));
            chaptersProvider.SetCurrentDificulty(dificulty);
        }
    }
}
