﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class ChooseInvestigatorGameAction : PhaseGameAction //2.2	Next investigator's turn begins.
    {
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly IPresenter<ChooseInvestigatorGameAction> _startingAnimationPresenter;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        public List<Effect> ChooseInvestigatorEffects { get; } = new();

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            foreach (Investigator investigator in _investigatorsProvider.GetInvestigatorsCanStart)
            {
                Effect newEffect = new(
                    investigator.AvatarCard,
                    investigator,
                    _textsProvider.GameText.DEFAULT_VOID_TEXT,
                    () => true,
                    ChooseInvestigator);
                _effectProvider.Add(newEffect);
                ChooseInvestigatorEffects.Add(newEffect);

                /*******************************************************************/
                async Task ChooseInvestigator()
                {
                    ActiveInvestigator = investigator;
                    await _startingAnimationPresenter.PlayAnimationWith(this);
                };
            }

            await _gameActionFactory.Create(new InteractableGameAction());
        }
    }
}