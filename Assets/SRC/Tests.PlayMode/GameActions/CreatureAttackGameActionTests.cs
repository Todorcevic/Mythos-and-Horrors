using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class CreatureAttackGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly CardsProvider _cardsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator CreatureAttackTest()
        {
            _prepareGameUseCase.Execute();
            CardCreature cardCreature = _cardsProvider.GetCard<CardCreature>("01119");
            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardCreature, _investigatorsProvider.Leader.DangerZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new CreatureAttackGameAction(cardCreature, _investigatorsProvider.Leader)).AsCoroutine();

            if (DEBUG_MODE) yield return PressAnyKey();
            Assert.That(_investigatorsProvider.Leader.Health.Value, Is.EqualTo(_investigatorsProvider.Leader.InvestigatorCard.Info.Health - 2));
            Assert.That(_investigatorsProvider.Leader.Sanity.Value, Is.EqualTo(_investigatorsProvider.Leader.InvestigatorCard.Info.Sanity - 1));
        }
    }
}
