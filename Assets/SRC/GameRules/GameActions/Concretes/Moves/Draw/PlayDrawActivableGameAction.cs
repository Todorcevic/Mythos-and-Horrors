using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayDrawActivableGameAction : GameAction
    {
        public IDrawActivable DrawActivable { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public PlayDrawActivableGameAction SetWith(IDrawActivable drawActivable, Investigator investigator)
        {
            DrawActivable = drawActivable;
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith((Card)DrawActivable, DrawActivable.ZoneToMoveWhenDraw(Investigator)).Execute();
            await DrawActivable.PlayRevelationFor(Investigator);
        }
    }
}
