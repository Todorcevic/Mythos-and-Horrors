using System.Collections.Generic;
using System.Threading.Tasks;

namespace MythsAndHorrors.GameRules
{
    public abstract class Scene
    {
        public string Code { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Card> Cards { get; set; }
        public string NextScene { get; set; }

        protected List<Card> PlaceCards => Cards.FindAll(card => card is CardPlace);
        protected List<Card> PlotCards => Cards.FindAll(card => card is CardPlot);
        protected List<Card> GoalCards => Cards.FindAll(card => card is CardGoal);
        protected List<Card> DangerCards => Cards.FindAll(card => card is CardAdversity || card is CardCreature);

        public abstract Task PrepareScene();
    }
}
