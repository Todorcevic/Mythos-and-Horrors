﻿using System.Collections.Generic;
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
            CardPlace currentPlace = Investigator.CurrentPlace;
            await _gameActionsProvider.Create(new MoveCardsGameAction(Investigator.AllCards, _chaptersProvider.CurrentScene.OutZone));
            await _gameActionsProvider.Create(new PayResourceGameAction(Investigator, Investigator.Resources.Value));
            await _gameActionsProvider.Create(new PayHintGameAction(Investigator, currentPlace.Hints, Investigator.Hints.Value));
            await _gameActionsProvider.Create(new MoveCardsGameAction(Investigator.CreaturesEnganged, currentPlace.OwnZone));
            await new SafeForeach<Card>(Discard, CardsInDangerZone).Execute();
            await eliminateInvestigatorPresenter.PlayAnimationWith(this);
        }

        private async Task Discard(Card card) => await _gameActionsProvider.Create(new DiscardGameAction(card));

        private IEnumerable<Card> CardsInDangerZone() => Investigator.DangerZone.Cards;
    }
}