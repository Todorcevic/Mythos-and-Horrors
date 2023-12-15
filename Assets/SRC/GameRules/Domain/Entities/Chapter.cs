using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class Chapter
    {
        public string Code { get; }
        public string Title { get; }
        public History Description { get; }
        public List<string> Adventurers { get; }
        public List<string> Scenes { get; }

        public bool HasThisScene(string sceneCode) => Scenes.Contains(sceneCode);
    }
}
