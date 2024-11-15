﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01182 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Omen };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, ChallengeToDiscardLogic, ChallengeToDiscardCondition, PlayActionType.Activate, new Localization("Activation_Card01182"));
            CreateBuff(CardsToBuff, ActivateBuff, DeactivateBuff, new Localization("Buff_Card01182"));
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => investigator.DangerZone;

        public override async Task PlayRevelationFor(Investigator investigator) => await Task.CompletedTask;

        /*******************************************************************/
        private async Task DeactivateBuff(IEnumerable<Card> cards)
        {
            CardInvestigator card = cards.OfType<CardInvestigator>().First();

            Dictionary<Stat, int> stats = new()
            {
                { card.Power, 1 }
            };
            if (card.Sanity.Value < card.Info.Sanity) stats.Add(card.Sanity, 1);

            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(stats).Execute();
        }

        private async Task ActivateBuff(IEnumerable<Card> cards)
        {
            CardInvestigator card = cards.OfType<CardInvestigator>().First();
            Dictionary<Stat, int> stats = new()
            {
                { card.Power, 1 }
            };
            if (card.Sanity.Value == card.Info.Sanity) stats.Add(card.Sanity, 1);

            await _gameActionsProvider.Create<DecrementStatGameAction>().SetWith(stats).Execute();
        }

        private IEnumerable<Card> CardsToBuff() => IsInPlay.IsTrue ? new[] { ControlOwner.InvestigatorCard } : new Card[0];

        /*******************************************************************/
        private async Task ChallengeToDiscardLogic(Investigator investigator)
        {
            await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Power, 3, new Localization("Challenge_Card01182", CurrentName), this, succesEffect: Discard).Execute();

            /*******************************************************************/
            async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        private bool ChallengeToDiscardCondition(Investigator investigator)
        {
            if (CurrentZone.ZoneType != ZoneType.Danger) return false;
            if (investigator.CurrentPlace != ControlOwner.CurrentPlace) return false;
            return true;
        }

    }
}
