using System.Collections.Generic;

namespace MythsAndHorrors.EditMode
{
    public class Chapter
    {
        public int Position { get; }
        public string Code { get; }
        public string Title { get; }
        public string Description { get; }
        public List<Adventurer> Adventurers { get; }
        public List<Scene> Scenaries { get; }
    }
}
