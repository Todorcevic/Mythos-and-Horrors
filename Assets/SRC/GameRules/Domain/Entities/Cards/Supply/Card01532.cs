using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01532 : CardSupply, IDamageable, IFearable
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;

        public Stat Health { get; private set; }
        public Stat DamageRecived { get; private set; }
        public Stat Sanity { get; private set; }
        public Stat FearRecived { get; private set; }
        public override IEnumerable<Tag> Tags => new[] { Tag.Ally, Tag.Miskatonic };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            Health = CreateStat(Info.Health ?? 0);
            DamageRecived = CreateStat(0);
            Sanity = CreateStat(Info.Sanity ?? 0);
            FearRecived = CreateStat(0);

            CreateOptativeReaction<MoveCardsGameAction>(Condition, Logic, GameActionTime.After);
        }

        /*******************************************************************/
        private async Task Logic(MoveCardsGameAction moveCardsGameAction)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Investigator");
            interactableGameAction.CreateCancelMainButton();

            IEnumerable<CardSupply> tomes = ControlOwner.DeckZone.Cards.OfType<CardSupply>().Where(card => card.HasThisTag(Tag.Tome));

            foreach (CardSupply tome in tomes)
            {
                interactableGameAction.CreateEffect(tome, new Stat(0, false), TakeTome, PlayActionType.Choose, ControlOwner);

                /*******************************************************************/
                async Task TakeTome()
                {
                    await _gameActionsProvider.Create(new DrawGameAction(ControlOwner, tome));
                    await _gameActionsProvider.Create(new HideCardsGameAction(tomes.Except(new[] { tome })));
                }
            }

            await _gameActionsProvider.Create(new ShowCardsGameAction(tomes));
            await interactableGameAction.Start();
            await _gameActionsProvider.Create(new ShuffleGameAction(ControlOwner.DeckZone));
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
