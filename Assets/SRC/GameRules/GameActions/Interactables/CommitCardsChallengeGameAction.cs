using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitCardsChallengeGameAction : InteractableGameAction, IPersonalInteractable
    {
        private const string CODE = "CommitCardsChallenge";
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly IPresenter<CommitCardsChallengeGameAction> _commitPresenter;

        public CardEffect ButtonEffect { get; private set; }
        public ChallengePhaseGameAction CurrentChallenge { get; private set; }

        private IEnumerable<CommitableCard> AllCommitableCards => _investigatorsProvider.GetInvestigatorsInThisPlace(CurrentChallenge.ActiveInvestigator.CurrentPlace)
              .SelectMany(investigator => investigator.HandZone.Cards)
              .OfType<CommitableCard>().Where(commitableCard => commitableCard.GetChallengeValue(CurrentChallenge.ChallengeType) > 0);

        public Investigator ActiveInvestigator => CurrentChallenge.ActiveInvestigator;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, string code)
        => throw new NotImplementedException();

        public CommitCardsChallengeGameAction SetWith(ChallengePhaseGameAction challenge)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, code: CODE);
            CurrentChallenge = challenge;
            ExecuteSpecificInitialization();
            return this;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            CreateMainButton(CurrentChallenge.ContinueChallenge, "Drop");

            foreach (CommitableCard commitableCard in AllCommitableCards)
            {
                CreateEffect(commitableCard, new Stat(0, false), Commit, PlayActionType.Commit, commitableCard.ControlOwner, cardAffected: CurrentChallenge.CardToChallenge);

                /*******************************************************************/
                async Task Commit()
                {
                    await _gameActionsProvider.Create<CommitGameAction>().SetWith(commitableCard).Execute();
                    await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(CurrentChallenge).Execute();
                }
            }

            foreach (CommitableCard commitableCard in _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<CommitableCard>().Where(commitable => commitable.Commited.IsActive))
            {
                CreateEffect(commitableCard, new Stat(0, false), Uncommit, PlayActionType.Commit, commitableCard.InvestigatorCommiter, cardAffected: CurrentChallenge.CardToChallenge);

                /*******************************************************************/
                async Task Uncommit()
                {
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(commitableCard, commitableCard.InvestigatorCommiter.HandZone).Execute();
                    await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(CurrentChallenge).Execute();
                }
            }

            foreach (Activation<ChallengePhaseGameAction> activation in _cardsProvider.AllCards.OfType<CardChallengeSupply>()
                .SelectMany(cardChallengeSupply => cardChallengeSupply.AllCommitsActivations)
                .Where(activation => activation.FullCondition(CurrentChallenge)))
            {
                CreateEffect(activation.Card,
                   activation.ActivateTurnsCost,
                   Activate,
                   PlayActionType.Activate | activation.PlayAction,
                   ActiveInvestigator);

                /*******************************************************************/
                async Task Activate()
                {
                    await activation.PlayFor(CurrentChallenge);
                    await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(CurrentChallenge).Execute();
                }
            }
        }

        /*******************************************************************/
        protected override async Task ExecuteThisLogic()
        {
            await _commitPresenter.PlayAnimationWith(this);
            await base.ExecuteThisLogic();
        }
    }
}

