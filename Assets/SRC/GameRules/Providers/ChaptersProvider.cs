using System;
using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{

    public class ChaptersProvider
    {
        private List<ChapterInfo> _chapters = new();

        public ChapterInfo CurrentChapter => _chapters.First(chapter => chapter.HasThisScene(CurrentScene.Info.Code));
        public Scene CurrentScene { get; private set; }
        public Dificulty CurrentDificulty { get; private set; }

        /*******************************************************************/
        public void AddChapters(List<ChapterInfo> chapters)
        {
            _chapters = chapters ?? throw new ArgumentNullException(nameof(chapters) + " chapters cant be null");
        }

        public ChapterInfo GetChapter(string chapterCode)
        {
            ChapterInfo chapter = _chapters.FirstOrDefault(chapter => chapter.Code == chapterCode);
            return chapter ?? throw new ArgumentException(nameof(chapterCode) + " chapter not found");
        }

        public void SetCurrentDificulty(Dificulty dificulty)
        {
            CurrentDificulty = dificulty;
        }

        public void SetCurrentScene(Scene scene)
        {
            CurrentScene = scene ?? throw new ArgumentNullException(nameof(scene) + " scene cant be null");
        }

    }
}
