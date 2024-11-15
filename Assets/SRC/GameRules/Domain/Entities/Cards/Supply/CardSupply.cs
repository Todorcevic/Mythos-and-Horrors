﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public abstract class CardSupply : CommitableCard, IPlayableFromHandInTurn
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public Stat ResourceCost { get; private set; }
        public GameConditionWith<Investigator> PlayFromHandCondition { get; private set; }
        public GameCommand<GameAction> PlayFromHandCommand { get; private set; }
        public PlayActionType PlayFromHandActionType => PlayActionType.PlayFromHand;

        /*******************************************************************/
        public CardPlace CurrentPlace => IsInPlay.IsTrue ? ControlOwner?.CurrentPlace ?? _cardsProvider.GetCardWithThisZone(CurrentZone) as CardPlace : null;
        public virtual Func<Card> CardAffected => null;

        public virtual bool IsFast => false;

        public bool HasAnyOfThisSlots(IEnumerable<SlotType> slotsType) => Info.Slots.Intersect(slotsType).Any();

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            ResourceCost = CreateStat(Info.Cost ?? 0);
            PlayFromHandCondition = new GameConditionWith<Investigator>(ConditionToPlayFromHand);
            PlayFromHandCommand = new GameCommand<GameAction>(PlayFromHand);
        }

        /*******************************************************************/
        private bool ConditionToPlayFromHand(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Hand) return false;
            if (ControlOwner != investigator) return false;
            return true;
        }

        private async Task PlayFromHand(GameAction investigator)
        {
            await _gameActionsProvider.Create<MoveCardsGameAction>()
                .SetWith(this, ControlOwner.AidZone).Execute();
        }
    }
}
