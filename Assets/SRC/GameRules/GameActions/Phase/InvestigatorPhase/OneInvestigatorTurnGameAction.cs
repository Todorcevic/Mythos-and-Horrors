﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class OneInvestigatorTurnGameAction : PhaseGameAction //2.2.1	Investigator takes an action, if able.
    {
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly TextsProvider _textsProvider;
        [Inject] private readonly ChaptersProvider _chapterProvider;

        public override string Name => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override string Description => _textsProvider.GameText.DEFAULT_VOID_TEXT;
        public override Phase MainPhase => Phase.Investigator;

        public Stat DrawCost { get; private set; }
        public List<Effect> MoveToPlaceEffects { get; } = new();
        public Effect DrawEffect { get; private set; }
        public Effect InvestigateEffect { get; private set; }
        public Effect TakeResourceEffect { get; private set; }
        public Effect PassEffect { get; private set; }
        public List<Effect> PlayFromHandEffects { get; } = new();

        /*******************************************************************/
        public OneInvestigatorTurnGameAction(Investigator investigator)
        {
            ActiveInvestigator = investigator;
            DrawCost = new Stat(1);
        }

        /*******************************************************************/
        protected override async Task ExecuteThisPhaseLogic()
        {
            PreparePassEffect();
            CheckIfCanMove();
            CheckIfCanInvestigate();
            CheckIfCanDraw();
            CheckIfCanTakeResource();
            CheckIsCanPlayFromHand();
            await _gameActionFactory.Create(new InteractableGameAction(PassEffect));
        }

        /*******************************************************************/
        private void PreparePassEffect()
        {
            PassEffect = new Effect(
                            null,
                            ActiveInvestigator,
                            _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(OneInvestigatorTurnGameAction),
                            () => true,
                            PassTurn);

            _effectProvider.Add(PassEffect);

            /*******************************************************************/
            async Task PassTurn() =>
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.Turns.Value));

        }

        private void CheckIsCanPlayFromHand()
        {
            foreach (Card card in ActiveInvestigator.HandZone.Cards)
            {
                if (card is IPlayableFromHand playableFromHand)
                {
                    Effect newEffect = new(
                        card,
                        ActiveInvestigator,
                        _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(PlayFromHand),
                        () => playableFromHand.CanPlayFromHand(),
                        PlayFromHand);
                    _effectProvider.Add(newEffect);
                    PlayFromHandEffects.Add(newEffect);

                    /*******************************************************************/
                    async Task PlayFromHand()
                    {
                        await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, playableFromHand.TurnsCost.Value));
                        await _gameActionFactory.Create(new PayResourceGameAction(ActiveInvestigator, playableFromHand.ResourceCost.Value));
                        await playableFromHand.PlayFromHand();
                    }
                }
            }
        }

        private void CheckIfCanTakeResource()
        {
            TakeResourceEffect = new(
                _chapterProvider.CurrentScene,
                ActiveInvestigator,
                _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(TakeResource),
                () => ActiveInvestigator.Turns.Value >= _chapterProvider.CurrentScene.ResourceCost.Value,
                TakeResource);

            _effectProvider.Add(TakeResourceEffect);

            /*******************************************************************/
            async Task TakeResource()
            {
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, _chapterProvider.CurrentScene.ResourceCost.Value));
                await _gameActionFactory.Create(new GainResourceGameAction(ActiveInvestigator, 1));
            }
        }

        private void CheckIfCanMove()
        {
            foreach (CardPlace connectedPlace in ActiveInvestigator.CurrentPlace.ConnectedPlacesToMove)
            {
                Effect newEffect = new(
                    connectedPlace,
                    ActiveInvestigator,
                    _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Move),
                    () => connectedPlace.CanMoveWithThis(ActiveInvestigator),
                    Move);
                MoveToPlaceEffects.Add(newEffect);
                _effectProvider.Add(newEffect);

                /*******************************************************************/
                async Task Move()
                {
                    await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, connectedPlace.MoveCost.Value));
                    await _gameActionFactory.Create(new MoveToPlaceGameAction(ActiveInvestigator, connectedPlace));
                }
            }
        }

        private void CheckIfCanInvestigate()
        {
            InvestigateEffect = new(
                ActiveInvestigator.CurrentPlace,
                ActiveInvestigator,
                _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Investigate),
                () => ActiveInvestigator.Turns.Value >= ActiveInvestigator.CurrentPlace.InvestigationCost.Value,
                Investigate);
            _effectProvider.Add(InvestigateEffect);

            /*******************************************************************/
            async Task Investigate()
            {
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, ActiveInvestigator.CurrentPlace.InvestigationCost.Value));
                await _gameActionFactory.Create(new InvestigateGameAction(ActiveInvestigator, ActiveInvestigator.CurrentPlace));
            }
        }

        private void CheckIfCanDraw()
        {
            DrawEffect = new(
                ActiveInvestigator.CardToDraw,
                ActiveInvestigator,
                _textsProvider.GameText.DEFAULT_VOID_TEXT + nameof(Draw),
               () => ActiveInvestigator.Turns.Value >= DrawCost.Value,
                Draw);
            _effectProvider.Add(DrawEffect);

            /*******************************************************************/
            async Task Draw()
            {
                await _gameActionFactory.Create(new DecrementStatGameAction(ActiveInvestigator.Turns, DrawCost.Value));
                await _gameActionFactory.Create(new DrawGameAction(ActiveInvestigator));
            }
        }
    }
}