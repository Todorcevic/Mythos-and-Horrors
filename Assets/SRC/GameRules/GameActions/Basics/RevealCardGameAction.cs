using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class RevealCardGameAction : GameAction
    {
        [Inject] private readonly ViewLayersProvider _viewLayerProvider;

        public IRevelable RevelableCard { get; }
        public Card Card => RevelableCard as Card;
        public bool IsPlaceReveled => RevelableCard is CardPlace;
        public bool IsPlotReveled => RevelableCard is CardPlot;
        public bool IsGoalReveled => RevelableCard is CardGoal;

        /*******************************************************************/
        public RevealCardGameAction(IRevelable revelableCard)
        {
            RevelableCard = revelableCard;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            RevelableCard.Reveal();
            await _viewLayerProvider.PlayAnimationWith(this);
        }
    }
}
