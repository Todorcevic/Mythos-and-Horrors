﻿using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class InvestigateGameAction : GameAction
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly IPresenter<InvestigateGameAction> _startingAnimationPresenter;

        public Investigator Investigator { get; }
        public CardPlace CardPlace { get; }

        /*******************************************************************/
        public InvestigateGameAction(Investigator investigator, CardPlace cardPlace)
        {
            Investigator = investigator;
            CardPlace = cardPlace;
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _startingAnimationPresenter.PlayAnimationWith(this);
            await _gameActionFactory.Create(new GainHintGameAction(Investigator, CardPlace.Hints, 1));
        }
    }
}