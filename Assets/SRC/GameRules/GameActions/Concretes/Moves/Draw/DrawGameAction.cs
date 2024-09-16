using System.Threading.Tasks;

namespace MythosAndHorrors.GameRules
{
    public class DrawGameAction : GameAction
    {
        public Investigator Investigator { get; private set; }
        public Card CardDrawed { get; protected set; }
        public override bool CanUndo => false;
        public override bool CanBeExecuted => Investigator.IsInPlay.IsTrue;

        /*******************************************************************/
        public DrawGameAction SetWith(Investigator investigator, Card cardDrawed)
        {
            Investigator = investigator;
            CardDrawed = cardDrawed;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await CheckRestore();
            await _gameActionsProvider.Create<ShowCardsGameAction>().SetWith(CardDrawed).Execute();

            switch (CardDrawed)
            {
                case IDrawRevelation cardAdversity:
                    await _gameActionsProvider.Create<PlayDrawRevelationGameAction>().SetWith(cardAdversity, Investigator).Execute();
                    break;
                case ISpawnable spawnable:
                    await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(spawnable).Execute();
                    break;
                case CardCreature cardCreature:
                    await _gameActionsProvider.Create<SpawnCreatureGameAction>().SetWith(cardCreature, Investigator).Execute();
                    break;
                default:
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(CardDrawed, Investigator.HandZone).Execute();
                    break;
            }

            await CheckRestore();
        }

        private async Task CheckRestore()
        {
            if (Investigator.CardAidToDraw == null) await _gameActionsProvider.Create<RestoreAidDeckGameAction>().SetWith(Investigator).Execute();
            if (Investigator.CardDangerToDraw == null) await _gameActionsProvider.Create<RestoreDangerDeckGameAction>().SetWith(Investigator).Execute();
        }
    }
}
