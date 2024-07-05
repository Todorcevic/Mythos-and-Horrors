using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class PlayDrawRevelationGameAction : GameAction
    {
        public IDrawRevelation DrawActivable { get; private set; }
        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public PlayDrawRevelationGameAction SetWith(IDrawRevelation drawRevelation, Investigator investigator)
        {
            DrawActivable = drawRevelation;
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
