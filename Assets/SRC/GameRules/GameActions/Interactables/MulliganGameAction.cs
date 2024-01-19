using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythsAndHorrors.GameRules
{
    public class MulliganGameAction : InteractableGameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;

        public override List<Card> ActivableCards => Investigator.HandZone.Cards.Concat(Investigator.DiscardZone.Cards).ToList();
        public Investigator Investigator { get; private set; }
        public override bool IsManadatary => false;

        /*******************************************************************/
        public async Task Run(Investigator investigator)
        {
            Investigator = investigator;
            await Start();
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await base.ExecuteThisLogic();

            if (CardSelected == null) return;
            if (CardSelected.CurrentZone == Investigator.HandZone)
                await _gameActionFactory.Create<DiscardGameAction>().Run(CardSelected);
            else if (CardSelected.CurrentZone == Investigator.DiscardZone)
                await _gameActionFactory.Create<MoveCardsGameAction>().Run(CardSelected, Investigator.HandZone);

            await _gameActionFactory.Create<MulliganGameAction>().Run(Investigator);
        }
    }
}

