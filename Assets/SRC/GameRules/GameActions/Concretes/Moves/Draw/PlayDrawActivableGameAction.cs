using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class PlayDrawActivableGameAction : GameAction
    {
        public IDrawActivable DrawActivable { get; }
        public Investigator Investigator { get; }

        /*******************************************************************/
        public PlayDrawActivableGameAction(IDrawActivable drawActivable, Investigator investigator)
        {
            DrawActivable = drawActivable;
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith((Card)DrawActivable, DrawActivable.ZoneToMoveWhenDraw(Investigator)).Start();
            await DrawActivable.PlayRevelationFor(Investigator);
        }
    }
}
