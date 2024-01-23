using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public async Task Run(Investigator investigator)
        {
            Investigator = investigator;
            await Start();
        }

        /*******************************************************************/
        protected sealed override async Task ExecuteThisLogic()
        {
            Investigator.HandZone.Cards.ForEach(card => card.AddEffect(Investigator, "Discard", MulliganDiscardEffect));
            Investigator.HandZone.Cards.ForEach(card => card.AddEffect(Investigator, "Restore", MulliganRestoreEffect));
            Investigator.DiscardZone.Cards.ForEach(card => card.AddEffect(Investigator, "Restore", MulliganRestoreEffect));

            await base.ExecuteThisLogic();
            if (NothingIsSelected) return;
            await _gameActionFactory.Create<MulliganGameAction>().Run(Investigator);
        }

        private Task MulliganDiscardEffect() => _gameActionFactory.Create<DiscardGameAction>().Run(CardSelected);

        private Task MulliganRestoreEffect() => _gameActionFactory.Create<MoveCardsGameAction>().Run(CardSelected, Investigator.HandZone);

    }
}

