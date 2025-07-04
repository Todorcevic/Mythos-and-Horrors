﻿using ModestTree;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01563 : CardSupply, IDamageable, IFearable, IEldritchable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public Stat Eldritch { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Sorcerer };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);
            Eldritch = CreateStat(0);
            CreateForceReaction<MoveCardsGameAction>(Condition, Logic, GameActionTime.After);
            CreateFastActivation(SearchLogic, SearchCondition, PlayActionType.Activate, new Localization("Activation_Card01563"));
        }

        private bool SearchCondition(Investigator investigator)
        {
            if (IsInPlay.IsFalse) return false;
            if (Exausted.IsActive) return false;
            return true;
        }

        private async Task SearchLogic(Investigator investigator)
        {
            IEnumerable<Card> cards = investigator.DeckZone.Cards.TakeLast(3);
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, new Localization("Interactable_Card01563"));

            Card cardSelected = null;

            foreach (Card card in cards.Where(card => card.HasThisTag(Tag.Spell)))
            {
                interactableGameAction.CreateCardEffect(card, new Stat(0, false), SelectSpell, PlayActionType.Choose, investigator, new Localization("CardEffect_Card01563"));

                /*******************************************************************/
                async Task SelectSpell()
                {
                    cardSelected = card;
                    await Task.CompletedTask;
                }
            }

            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(Exausted, true).Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cards, _chaptersProvider.CurrentScene.LimboZone).Execute();
            await interactableGameAction.Execute();
            await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(cards.Except(cardSelected), investigator.DeckZone, isFaceDown: true).Execute();
            await _gameActionsProvider.Create<ShuffleGameAction>().SetWith(investigator.DeckZone).Execute();
            if (cardSelected != null) await _gameActionsProvider.Create<DrawGameAction>().SetWith(investigator, cardSelected).Execute();
        }

        /*******************************************************************/
        private async Task Logic(MoveCardsGameAction moveCardsGameAction)
        {
            await _gameActionsProvider.Create<IncrementStatGameAction>().SetWith(Eldritch, 1).Execute();
        }

        private bool Condition(MoveCardsGameAction moveCardsGameAction)
        {
            if (!moveCardsGameAction.Cards.Contains(this)) return false;
            if (moveCardsGameAction.AllMoves[this].zone.ZoneType != ZoneType.Aid) return false;
            if (ZoneType.PlayZone.HasFlag(moveCardsGameAction.GetZoneBeforeMoveFor(this).ZoneType)) return false;
            return true;
        }
    }
}
