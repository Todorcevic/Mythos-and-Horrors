﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Zenject;

namespace MythosAndHorrors.GameRules
{
    public class CommitCardsChallengeGameAction : InteractableGameAction, IPersonalInteractable
    {
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        public ChallengePhaseGameAction CurrentChallenge { get; private set; }

        private IEnumerable<CommitableCard> AllCommitableCards => _investigatorsProvider.GetInvestigatorsInThisPlace(CurrentChallenge.ActiveInvestigator.CurrentPlace)
              .SelectMany(investigator => investigator.HandZone.Cards)
              .OfType<CommitableCard>().Where(commitableCard => commitableCard.GetChallengeFullValueWithWild(CurrentChallenge.ChallengeType) > 0);

        public Investigator ActiveInvestigator => CurrentChallenge.ActiveInvestigator;

        /*******************************************************************/
        [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Parent method must be hide")]
        private new InteractableGameAction SetWith(bool canBackToThisInteractable, bool mustShowInCenter, Localization localization) => throw new NotImplementedException();

        public CommitCardsChallengeGameAction SetWith(ChallengePhaseGameAction challenge, Func<Task> ContinueMainButton)
        {
            base.SetWith(canBackToThisInteractable: true, mustShowInCenter: false, new Localization("Interactable_CommitCardsChallenge"));
            CurrentChallenge = challenge;
            ExecuteSpecificInitialization();
            CreateMainButton(ContinueMainButton, new Localization("MainButton_CommitCardsChallenge"));

            return this;
        }

        /*******************************************************************/
        private void ExecuteSpecificInitialization()
        {
            foreach (CommitableCard commitableCard in AllCommitableCards)
            {
                CreateCardEffect(commitableCard, new Stat(0, false), Commit, PlayActionType.Commit,
                    commitableCard.ControlOwner, new Localization("CardEffect_CommitCardsChallenge"), cardAffected: CurrentChallenge.CardToChallenge);

                /*******************************************************************/
                async Task Commit()
                {
                    await _gameActionsProvider.Create<CommitGameAction>().SetWith(commitableCard).Execute();
                    await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(CurrentChallenge, MainButtonEffect.Logic).Execute();
                }
            }

            foreach (CommitableCard commitableCard in _chaptersProvider.CurrentScene.LimboZone.Cards.OfType<CommitableCard>().Where(commitable => commitable.Commited.IsActive))
            {
                CreateCardEffect(commitableCard, new Stat(0, false), Uncommit, PlayActionType.Commit,
                    commitableCard.InvestigatorCommiter, new Localization("CardEffect_CommitCardsChallenge-1"), cardAffected: CurrentChallenge.CardToChallenge);

                /*******************************************************************/
                async Task Uncommit()
                {
                    await _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(commitableCard, commitableCard.InvestigatorCommiter.HandZone).Execute();
                    await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(CurrentChallenge, MainButtonEffect.Logic).Execute();
                }
            }

            foreach (Activation<ChallengePhaseGameAction> activation in _cardsProvider.AllCards.OfType<CardChallengeSupply>()
                .SelectMany(cardChallengeSupply => cardChallengeSupply.AllCommitsActivations)
                .Where(activation => activation.FullCondition(CurrentChallenge)))
            {
                CreateCardEffect(activation.Card,
                   activation.ActivateActionsCost,
                   Activate,
                   PlayActionType.Activate | activation.PlayAction,
                   ActiveInvestigator,
                   activation.Localization);

                /*******************************************************************/
                async Task Activate()
                {
                    await activation.PlayFor(CurrentChallenge);
                    await _gameActionsProvider.Create<CommitCardsChallengeGameAction>().SetWith(CurrentChallenge, MainButtonEffect.Logic).Execute();
                }
            }
        }
    }
}

