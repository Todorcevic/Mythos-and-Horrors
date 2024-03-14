using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class MultiEffectTests : TestBase
    {
        [Inject] private readonly EffectsProvider _effectProvider;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zoneViewManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MultiEffect_Test()
        {
            _prepareGameUseCase.Execute();
            Investigator investigator1 = _investigatorsProvider.Leader;
            Card card = investigator1.Cards[1];
            Card card2 = investigator1.Cards[2];

            _effectProvider.CreateMainButton()
                     .SetDescription("Continue")
                     .SetLogic(() => Task.CompletedTask);

            _effectProvider.Create()
                .SetCard(card)
                .SetInvestigator(investigator1)
                .SetDescription("EffectOne")
                .SetLogic(() => _gameActionFactory.Create(new MoveCardsGameAction(card, investigator1.DangerZone)));

            _effectProvider.Create()
                .SetCard(card)
                .SetInvestigator(investigator1)
                .SetDescription("EffectTwo")
                .SetLogic(() => _gameActionFactory.Create(new MoveCardsGameAction(card, investigator1.HandZone)));

            _effectProvider.Create()
                .SetCard(card2)
                .SetInvestigator(investigator1)
                .SetDescription("EffectOne")
                .SetLogic(() => _gameActionFactory.Create(new MoveCardsGameAction(card2, investigator1.DangerZone)));

            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards.Take(5).ToList(), investigator1.HandZone)).AsCoroutine();
            if (!DEBUG_MODE) WaitToClick2(card).AsTask();
            yield return _gameActionFactory.Create(new InteractableGameAction()).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(investigator1.DangerZone.TopCard, Is.EqualTo(card));
        }

        private IEnumerator WaitToClick2(Card card)
        {
            CardSensorController cardSensor = _cardViewsManager.GetCardView(card).GetComponentInChildren<CardSensorController>();
            yield return new WaitUntil(() => cardSensor.IsClickable);
            cardSensor.OnMouseUpAsButton();
            yield return new WaitUntil(() => cardSensor.IsClickable && _zoneViewManager.SelectorZone.GetComponentsInChildren<CardView>().Length > 0);
            cardSensor.OnMouseUpAsButton();
        }
    }
}
