using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EliminateInvestigatorGameAction : GameAction
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IPresenter<EliminateInvestigatorGameAction> eliminateInvestigatorPresenter;

        public Investigator Investigator { get; }

        /*******************************************************************/
        public EliminateInvestigatorGameAction(Investigator investigator)
        {
            Investigator = investigator;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _gameActionsProvider.Create(new PayResourceGameAction(Investigator, Investigator.Resources.Value));
            await _gameActionsProvider.Create(new DropHintGameAction(Investigator, Investigator.CurrentPlace.Hints, Investigator.Hints.Value));

            Dictionary<Card, Zone> moveAndDisconfront = Investigator.BasicCreaturesConfronted
                .ToDictionary(creature => (Card)creature, creature => creature.CurrentPlace.OwnZone);
            moveAndDisconfront.Concat(Investigator.Cards.ToDictionary(card => card, card => _chaptersProvider.CurrentScene.OutZone));

            await _gameActionsProvider.Create(new MoveCardsGameAction(moveAndDisconfront));

            await _gameActionsProvider.Create(new SafeForeach<Card>(() => Investigator.DangerZone.Cards, Discard));
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);
        }

        private async Task Discard(Card card) => await _gameActionsProvider.Create(new DiscardGameAction(card));

        public override async Task Undo()
        {
            await base.Undo();
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);
        }
    }
}
