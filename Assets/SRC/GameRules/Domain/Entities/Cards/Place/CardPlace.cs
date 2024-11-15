﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardPlace : Card, IRevealable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public Stat Keys { get; private set; }
        public Stat Enigma { get; private set; }
        public State Revealed { get; private set; }
        public Conditional CanBeInvestigated { get; private set; }
        public Conditional CanMoveHere { get; protected set; }
        public GameCommand<RevealGameAction> RevealCommand { get; private set; }

        /*******************************************************************/
        public int MaxKeys => (Info.Keys ?? 0) * _investigatorsProvider.AllInvestigators.Count();
        public bool IsAlone => !OwnZone.Cards.Any(card => card is CardAvatar || card is CardCreature);
        public History RevealHistory => Info.Histories?.ElementAt(0) ?? new History(); //TODO: Remove control when all cards have history
        public IEnumerable<CardCreature> CreaturesInThisPlace => _cardsProvider.GetCardsInPlay().OfType<CardCreature>()
            .Where(creature => creature.CurrentPlace == this);
        public IEnumerable<Investigator> InvestigatorsInThisPlace => _investigatorsProvider.AllInvestigatorsInPlay
            .Where(investigator => investigator.CurrentPlace == this);
        public IEnumerable<CardPlace> ConnectedPlacesToMove => Info.ConnectedPlaces?.Select(code => _cardsProvider.GetCardByCode(code)).Cast<CardPlace>()
            .Where(cardPlace => cardPlace.CanMoveHere.IsTrue);
        public IEnumerable<CardPlace> ConnectedPlacesFromMove => _cardsProvider.GetCardsThatCanMoveTo(this);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Keys = CreateStat(MaxKeys);
            Enigma = CreateStat(Info.Enigma ?? 0);
            Revealed = CreateState(false);
            RevealCommand = new GameCommand<RevealGameAction>(RevealEffect);
            CanBeInvestigated = new Conditional(() => IsInPlay.IsTrue && Revealed.IsActive);
            CanMoveHere = new Conditional(() => IsInPlay.IsTrue);
            CreateBaseReaction<MoveCardsGameAction>(RevealCondition, RevealLogic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task RevealEffect(RevealGameAction revealGameAction)
        {
            await _gameActionsProvider.Create<ShowHistoryGameAction>().SetWith(RevealHistory, this).Execute();
        }

        /*******************************************************************/
        private bool RevealCondition(MoveCardsGameAction moveCardsGameAction)
        {
            if (Revealed.IsActive) return false;
            if (!OwnZone.Cards.Any(card => card is CardAvatar)) return false;
            return true;
        }

        private async Task RevealLogic(MoveCardsGameAction moveCardsGameAction) =>
            await _gameActionsProvider.Create<RevealGameAction>().SetWith(this).Execute();

    }
}
