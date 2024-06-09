using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01538 : CardCondition
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
            CreateReaction<MoveInvestigatorToPlaceGameAction>(DiscardCondition, DiscardLogic, isAtStart: false);
            CreateReaction<MoveInvestigatorToPlaceGameAction>(ConforntCantMoveCondition, ConfrontCantMoveLogic, isAtStart: false);
            CreateReaction<MoveCreatureGameAction>(CantMoveCondition, CantMoveLogic, isAtStart: true);
        }

        /*******************************************************************/
        private async Task ConfrontCantMoveLogic(MoveInvestigatorToPlaceGameAction moveInvestigatorGameAction)
        {
            IEnumerable<CardCreature> allCreatures = moveInvestigatorGameAction.Investigators.SelectMany(investigator => investigator.BasicCreaturesConfronted);
            Dictionary<Card, Zone> allMoves = allCreatures.ToDictionary(creature => (Card)creature, creature => moveInvestigatorGameAction.From[creature.ConfrontedInvestigator].OwnZone);
            await _gameActionsProvider.Create(new MoveCardsGameAction(allMoves));
        }

        private bool ConforntCantMoveCondition(MoveInvestigatorToPlaceGameAction moveInvestigatorGameAction)
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
    }
}
