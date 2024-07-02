using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class EliminateInvestigatorGameAction : GameAction
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly IPresenter<EliminateInvestigatorGameAction> eliminateInvestigatorPresenter;

        public Investigator Investigator { get; private set; }

        /*******************************************************************/
        public EliminateInvestigatorGameAction SetWith(Investigator investigator)
        {
            Investigator = investigator;
            return this;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            CardPlace currentPlace = Investigator.CurrentPlace;
            await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(Investigator, Investigator.Resources.Value).Start();
            await _gameActionsProvider.Create<DropHintGameAction>().SetWith(Investigator, currentPlace.Hints, Investigator.Hints.Value).Start();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Investigator.Cards, _chaptersProvider.CurrentScene.OutZone).Start();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Investigator.BasicCreaturesConfronted, currentPlace.OwnZone).Start();
            await _gameActionsProvider.Create<SafeForeach<Card>>().SetWith(DangerCards, Discard).Start();
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);

            /*******************************************************************/
            async Task Discard(Card card) => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Start();
            IEnumerable<Card> DangerCards() => Investigator.DangerZone.Cards;
        }

        public override async Task Undo()
        {
            await base.Undo();
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);
        }
    }
}
