using System.Collections.Generic;

namespace MythsAndHorrors.EditMode
{
    public class Scene
    {
        public int Position { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Card> CardsCode { get; set; }
    }
}
