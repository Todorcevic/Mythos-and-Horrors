using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using Zenject;

namespace MythosAndHorrors.EditMode.Tests
{
    public abstract class SetupAutoInject
    {
        [Inject] protected readonly PrepareGameRulesUseCase _prepareGameRulesUseCase;
        [Inject] protected readonly ChaptersProvider _chaptersProvider;
        [Inject] protected readonly GameActionsProvider _gameActionsProvider;
        [Inject] protected readonly CardsProvider _cardsProvider;
        [Inject] protected readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] protected readonly BuffsProvider _buffsProvider;
        [Inject] protected readonly ReactionablesProvider _reactionablesProvider;
        [Inject] protected readonly ChallengeTokensProvider _challengeTokensProvider;

        protected DiContainer Container { get; private set; }

        /*******************************************************************/
        [SetUp]
        public virtual void RunBeforeAnyTest()
        {
            Container = new();
            Container.Install<InjectionService>();
            Container.Inject(this);
        }

        [TearDown]
        public virtual void RunAfterAnyTest()
        {
            Container = null;
        }
    }
}