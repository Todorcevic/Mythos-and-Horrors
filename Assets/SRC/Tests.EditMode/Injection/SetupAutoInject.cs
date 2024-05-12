using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        [Inject] private readonly IInteractablePresenter interactablePresenter;

        protected FakeInteractablePresenter InteractablePresenter => (FakeInteractablePresenter)interactablePresenter;
        protected DiContainer Container { get; private set; }

        /*******************************************************************/
        [SetUp]
        public virtual void RunBeforeAnyTest()
        {
            Container = new();
            Container.Install<InjectionService>();
            BindContainer();
            Container.Inject(this);
        }

        [TearDown]
        public virtual void RunAfterAnyTest()
        {
            Container = null;
        }

        protected virtual void BindContainer()
        {
            Container.Rebind<IInteractablePresenter>().To<FakeInteractablePresenter>().AsCached();
            BindAllFakePresenters();
        }

        private void BindAllFakePresenters()
        {
            IEnumerable<Type> gameActionTypes = typeof(GameAction).Assembly.GetTypes()
                .Where(type => type.IsClass && (type.BaseType == typeof(GameAction)));

            foreach (Type type in gameActionTypes)
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                FieldInfo[] campos = type.GetFields(flags);

                foreach (FieldInfo campo in campos)
                {
                    if (campo.FieldType.IsGenericType &&
                        campo.FieldType.GetGenericTypeDefinition() == typeof(IPresenter<>) && campo.FieldType.GetGenericArguments()[0] == type)
                    {
                        Type genericToBind = typeof(FakePresenter<>).MakeGenericType(type);
                        Container.Rebind(campo.FieldType).To(genericToBind).AsCached();
                    }
                }
            }
        }
    }
}