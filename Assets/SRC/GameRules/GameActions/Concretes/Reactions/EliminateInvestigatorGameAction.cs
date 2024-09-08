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
            await _gameActionsProvider.Create<PayResourceGameAction>().SetWith(Investigator, Investigator.Resources.Value).Execute();
            await _gameActionsProvider.Create<DropKeyGameAction>().SetWith(Investigator, currentPlace.Keys, Investigator.Keys.Value).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Investigator.Cards, _chaptersProvider.CurrentScene.OutZone).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(Investigator.BasicCreaturesConfronted, currentPlace.OwnZone).Execute();
            await _gameActionsProvider.Create<SafeForeach<Card>>().SetWith(DangerCards, Discard).Execute();
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);

            /*******************************************************************/

            IEnumerable<Card> DangerCards() => Investigator.DangerZone.Cards;

            async Task Discard(Card card) => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
        }

        public override async Task Undo()
        {
            await base.Undo();
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);
        }
    }
}
