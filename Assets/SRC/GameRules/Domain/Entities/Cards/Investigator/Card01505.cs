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
            StarTokenDescription = () => ExtraInfo.StarTokenDescription.ParseViewWith(AmuletoDeWendy.Info.Name);
            AbilityUsed = CreateState(false);
            CreateOptativeReaction<RevealChallengeTokenGameAction>(RevealNewTokenCondition, RevealNewTokenLogic, GameActionTime.After, "OptativeReaction_Card01505");
            CreateForceReaction<ChallengePhaseGameAction>(RestartAbilityCondition, RestartAbilityLogic, GameActionTime.Before);
        }

        /*******************************************************************/
        private async Task RevealNewTokenLogic(RevealChallengeTokenGameAction revealChallengeToken)
        {
            InteractableGameAction interactableGameAction = _gameActionsProvider.Create<InteractableGameAction>()
                .SetWith(canBackToThisInteractable: false, mustShowInCenter: true, "Interactable_Card01505");

            foreach (Card card in Owner.DiscardableCardsInHand)
            {
                interactableGameAction.CreateCardEffect(card, new Stat(0, false), Discard, PlayActionType.Choose, playedBy: Owner, "CardEffect_Card01505");

                /*******************************************************************/
                async Task Discard()
                {
                    await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, true).Execute();
                    await _gameActionsProvider.Create<DiscardGameAction>().SetWith(card).Execute();
                    await _gameActionsProvider.Create<RestoreChallengeTokenGameAction>().SetWith(revealChallengeToken.ChallengeTokenRevealed).Execute();
                    await _gameActionsProvider.Create<RevealRandomChallengeTokenGameAction>().SetWith(Owner).Execute();
                }
            }

            await interactableGameAction.Execute();
        }

        private bool RevealNewTokenCondition(RevealChallengeTokenGameAction revealChallengeTokenGameAction)
        {
            if (!IsInPlay.IsTrue) return false;
            if (revealChallengeTokenGameAction.Investigator != Owner) return false;
            if (!Owner.HandZone.Cards.Any(card => card.CanBeDiscarted.IsTrue)) return false;
            if (AbilityUsed.IsActive) return false;
            return true;
        }

        /*******************************************************************/
        protected override async Task StarEffect()
        {
            if (!AmuletoDeWendy.IsInPlay.IsTrue) return;
            _gameActionsProvider.CurrentChallenge.IsAutoSucceed = true;
            await Task.CompletedTask;
        }

        protected override int StarValue() => 0;

        /*******************************************************************/
        private async Task RestartAbilityLogic(ChallengePhaseGameAction challengePhaseGameAction)
        {
            await _gameActionsProvider.Create<UpdateStatesGameAction>().SetWith(AbilityUsed, false).Execute();
        }

        private bool RestartAbilityCondition(ChallengePhaseGameAction challengePhaseGameAction)
        {
            if (!AbilityUsed.IsActive) return false;
            return true;
        }
    }
}
