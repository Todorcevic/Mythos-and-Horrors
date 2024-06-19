using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01538 : CardConditionPlayFromHand
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Tactic, Tag.Insight };

        protected override bool IsFast => false;
        private CardPlace CurrentPlace => _cardsProvider.GetCards<CardPlace>().First(place => place.OwnZone == CurrentZone);

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(DiscardCondition, DiscardLogic, GameActionTime.After);
            CreateForceReaction<MoveInvestigatorToPlaceGameAction>(ConfrontCantMoveCondition, ConfrontCantMoveLogic, GameActionTime.Before);
            CreateForceReaction<MoveCreatureGameAction>(CantMoveCondition, CantMoveLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task ConfrontCantMoveLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorGameAction)
        {
            if (moveInvestigatorGameAction is not MoveInvestigatorAndUnconfrontGameAction)
            {
                moveInvestigatorGameAction.Cancel();
                await _gameActionsProvider.Create(new MoveInvestigatorAndUnconfrontGameAction(moveInvestigatorGameAction.Investigators, moveInvestigatorGameAction.CardPlace));
            }
        }

        private bool ConfrontCantMoveCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorGameAction)
        {
            if (!IsInPlay) return false;
            if (moveInvestigatorGameAction.CardPlace != CurrentPlace) return false;
            if (!moveInvestigatorGameAction.Investigators.Any(investigator => investigator.BasicCreaturesConfronted.Any())) return false;
            return true;
        }

        /*******************************************************************/
        private async Task CantMoveLogic(MoveCreatureGameAction moveCreatureGameAction)
        {
            moveCreatureGameAction.Cancel();
            await Task.CompletedTask;
        }

        private bool CantMoveCondition(MoveCreatureGameAction moveCreatureGameAction)
        {
            if (!IsInPlay) return false;
            if (moveCreatureGameAction.Creature.HasThisTag(Tag.Elite)) return false;
            if (moveCreatureGameAction.Destiny != CurrentPlace) return false;
            return true;
        }

        /*******************************************************************/
        private async Task DiscardLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorCardGameAction)
        {
            await _gameActionsProvider.Create(new DiscardGameAction(this));
        }

        private bool DiscardCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorCardGameAction)
        {
            if (!IsInPlay) return false;
            if (!moveInvestigatorCardGameAction.From.Values.Contains(CurrentPlace)) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task ExecuteConditionEffect(Investigator investigator)
        {
            await _gameActionsProvider.Create(new MoveCardsGameAction(this, investigator.CurrentPlace.OwnZone));
        }

        protected override bool CanPlayFromHandSpecific(GameAction gameAction) => true;

    }
}
