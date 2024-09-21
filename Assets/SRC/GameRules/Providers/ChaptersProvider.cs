using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChaptersProvider
    {
        [Inject] private readonly OwnersProvider _ownersProvider;
        private List<Chapter> _chapters = new();

        public Chapter CurrentChapter => _chapters.First(chapter => chapter.HasThisScene(CurrentScene.Code));
        public Scene CurrentScene => _ownersProvider.AllOwners.OfType<Scene>().First();
        public Dificulty CurrentDificulty { get; private set; }

        /*******************************************************************/
        public void AddChapters(List<Chapter> chapters)
        {
            _chapters = chapters ?? throw new ArgumentNullException(nameof(chapters) + " chapters cant be null");
        }

        public Chapter GetChapter(string chapterCode)
        {
            Chapter chapter = _chapters.FirstOrDefault(chapter => chapter.Code == chapterCode);
            return chapter ?? throw new ArgumentException(nameof(chapterCode) + " chapter not found");
        }

        public void SetCurrentDificulty(Dificulty dificulty)
        {
            CurrentDificulty = dificulty;
        }
    }
}
