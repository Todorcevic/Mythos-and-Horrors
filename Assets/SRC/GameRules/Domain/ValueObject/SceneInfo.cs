using System.Collections.Generic;
using System.Linq;

namespace MythsAndHorrors.GameRules
{
    public record SceneInfo
    {
        public string Code { get; init; }
        public int Position { get; init; }
        public string Name { get; init; }
        public History Description { get; init; }
        public List<History> Resolutions { get; init; }
        public List<Card> Cards { get; init; }
        public string NextScene { get; init; }

        public List<CardPlace> PlaceCards => Cards.OfType<CardPlace>().ToList();
        public List<CardPlot> PlotCards => Cards.OfType<CardPlot>().ToList();
        public List<CardGoal> GoalCards => Cards.OfType<CardGoal>().ToList();
        public List<Card> DangerCards => Cards.FindAll(card => card is CardAdversity || card is CardCreature);
    }
}
