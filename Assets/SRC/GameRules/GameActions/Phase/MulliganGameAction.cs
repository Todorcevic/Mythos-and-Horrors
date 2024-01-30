using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MulliganGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public MulliganGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected sealed override async Task ExecuteThisLogic()
        {
            Investigator.HandZone.Cards.ForEach(card => card.AddEffect(Investigator, "Discard", () => MulliganDiscardEffect(card)));
            Investigator.DiscardZone.Cards.FindAll(card => card is not IWeakness).ForEach(card => card.AddEffect(Investigator, "Restore", () => MulliganRestoreEffect(card, Investigator.HandZone)));

            InteractableGameAction basicPlay = await _gameActionFactory.Create(new InteractableGameAction(false));

            if (basicPlay.NothingIsSelected) return;
            await _gameActionFactory.Create(new MulliganGameAction(Investigator));
        }

        private Task MulliganDiscardEffect(Card card) => _gameActionFactory.Create<DiscardGameAction>().Run(card);

        private Task MulliganRestoreEffect(Card card, Zone zone) => _gameActionFactory.Create<MoveCardsGameAction>().Run(card, zone);
    }
}

