using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class Card01505 : CardInvestigator
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly CardsProvider _cardsProvider;

        public State AbilityUsed { get; private set; }
        private Card AmuletoDeWendy => _cardsProvider.GetCard<Card01514>();
        public override IEnumerable<Tag> Tags => new[] { Tag.Drifter };

        /*******************************************************************/
        [Inject]
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Zenject injects this method")]
        private void Init()
        {
            AbilityUsed = CreateState(false);
            CreateOptativeReaction<RevealChallengeTokenGameAction>(RevealNewTokenCondition, RevealNewTokenLogic, GameActionTime.After);
            CreateForceReaction<ChallengePhaseGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task RevealNewTokenLogic(RevealChallengeTokenGameAction revealChallengeToken)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Select to Discard");

            foreach (Card card in Owner.DiscardableCardsInHand)
            {
                interactableGameAction.CreateEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: Owner);

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Start();
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Start();
                    await _gameActionsProvider.Create(new RestoreChallengeTokenGameAction(revealChallengeToken.ChallengeTokenRevealed));
                    await _gameActionsProvider.Create(new RevealRandomChallengeTokenGameAction(Owner));
                }
            }

            await interactableGameAction.Start();
        }

        private bool RevealNewTokenCondition(RevealChallengeTokenGameAction revealChallengeTokenGameAction)
        {
            if (!IsInPlay) return false;
            if (revealChallengeTokenGameAction.Investigator != Owner) return false;
            if (!Owner.HandZone.Cards.Any(card => card.CanBeDiscarded)) return false;
            if (AbilityUsed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task StarEffect()
        {
            if (!AmuletoDeWendy.IsInPlay) return;
            _gameActionsProvider.CurrentChallenge.IsAutoSucceed = true;
            await Task.CompletedTask;
        }

        protected override int StarValue() => 0;

        /*******************************************************************/
        private async Task RestartAbilityLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Start();
        }

        private bool RestartAbilityCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
