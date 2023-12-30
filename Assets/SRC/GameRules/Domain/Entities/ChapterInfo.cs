using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public record ChapterInfo
    {
        public string Code { get; init; }
        public string Title { get; init; }
        public History Description { get; init; }
        public List<string> Investigators { get; init; }
        public List<string> Scenes { get; init; }

        public bool HasThisScene(string sceneCode) => Scenes.Contains(sceneCode);
    }
}
