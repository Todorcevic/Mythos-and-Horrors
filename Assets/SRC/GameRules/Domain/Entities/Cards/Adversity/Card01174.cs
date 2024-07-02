using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01174 : CardAdversity
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;

        public override IEnumerable<Tag> Tags => new[] { Tag.Obstacle };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected by Zenject")]
        private void Init()
        {
            CreateActivation(1, TryOpenLogic, TryOpenCondition, PlayActionType.Activate);
            CreateForceReaction<InteractableGameAction>(AvoidInvestigateCondition, AvoidInvestigateLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        public override sealed Zone ZoneToMoveWhenDraw(Investigator investigator) => _chaptersProvider.CurrentScene.PlaceCards.Where(place => !place.OwnZone.Cards.Exists(card => card is Card01174))
                .OrderByDescending(place => place.Revealed.IsActive).ThenByDescending(place => place.Hints.Value).FirstOrDefault()?.OwnZone
                ?? _chaptersProvider.CurrentScene.LimboZone;

        public override async Task PlayRevelationFor(Investigator investigator)
        {
            if (CurrentZone == _chaptersProvider.CurrentScene.LimboZone) await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();
        }

        /*******************************************************************/
        private async Task AvoidInvestigateLogic(InteractableGameAction interactableGameAction)
        {
            Card placeAffected = _cardsProvider.GetCardWithThisZone(CurrentZone);
            IEnumerable<CardEffect> investigateEffects = interactableGameAction.AllEffects
               .Where(effects => effects.IsActionType(PlayActionType.Investigate) && (effects.CardOwner == placeAffected || effects.CardAffected == placeAffected));
            interactableGameAction.RemoveEffects(investigateEffects);
            await Task.CompletedTask;
        }

        private bool AvoidInvestigateCondition(InteractableGameAction interactableGameAction)
        {
            if (!IsInPlay) return false;
            return true;
        }

        private IEnumerable<Card> CardsToBuff()
        {
            Card card = _cardsProvider.GetCardWithThisZone(CurrentZone);
            return card != null ? new[] { card } : Enumerable.Empty<Card>();
        }

        /*******************************************************************/
        private async Task TryOpenLogic(Investigator investigator)
        {
            InteractableGameAction choose = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Choose Challenge");
            choose.CreateEffect(this, new Stat(0, false), StrengthChallenge, PlayActionType.Choose, playedBy: investigator);
            choose.CreateEffect(this, new Stat(0, false), AgilityChallenge, PlayActionType.Choose, playedBy: investigator);
            await choose.Execute();

            /*******************************************************************/
            async Task StrengthChallenge() => await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Strength, 4, "Open", this, succesEffect: Discard).Execute();
            async Task AgilityChallenge() => await _gameActionsProvider.Create<ChallengePhaseGameAction>().SetWith(investigator.Agility, 4, "Open", this, succesEffect: Discard).Execute();

            /*******************************************************************/
            async Task Discard() => await _gameActionsProvider.Create<DiscardGameAction>().SetWith(this).Execute();

        }

        private bool TryOpenCondition(Investigator investigator)
        {
            if (investigator.CurrentPlace.OwnZone != CurrentZone) return false;
            return true;
        }
    }
}
