using System.Collections.Generic;

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

        public List<Card> PlaceCards => Cards.FindAll(card => card is CardPlace);
        public List<Card> PlotCards => Cards.FindAll(card => card is CardPlot);
        public List<Card> GoalCards => Cards.FindAll(card => card is CardGoal);
        public List<Card> DangerCards => Cards.FindAll(card => card is CardAdversity || card is CardCreature);
    }
}
