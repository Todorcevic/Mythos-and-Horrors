using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class Chapter
    {
        public int Position { get; }
        public string Code { get; }
        public string Title { get; }
        public string Description { get; }
        public List<string> PlayerCardsCode { get; }
        public List<Scene> Scenaries { get; }
    }
}
