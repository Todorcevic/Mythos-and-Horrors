using System.Collections.Generic;

namespace MythsAndHorrors.GameRules
{
    public class Scene
    {
        public int Position { get; }
        public string Name { get; }
        public string Description { get; }
        public List<string> CardsCode { get; }
    }
}
