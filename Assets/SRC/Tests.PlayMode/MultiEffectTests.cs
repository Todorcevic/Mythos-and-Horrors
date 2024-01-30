using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class MultiEffectTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;
        [Inject] private readonly GameActionFactory _gameActionFactory;
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
            card.AddEffect(investigator1, "EffectOne En un lugar de la Mancha Cuyo sin es la carretera", () => _gameActionFactory.Create(new MoveCardsGameAction(card, investigator1.DangerZone)));
            card.AddEffect(null, "EffectTwo En un lugar de la Mancha Cuyo sin es la carretera", () => _gameActionFactory.Create(new MoveCardsGameAction(card, investigator1.HandZone)));
            card2.AddEffect(investigator1, "EffectOne En un lugar de la Mancha Cuyo sin es la carretera", () => _gameActionFactory.Create(new MoveCardsGameAction(card2, investigator1.DangerZone)));
            //investigator1.Cards[3].AddEffect(investigator1, "EffectOne En un lugar de la Mancha Cuyo sin es la carretera", () => _gameActionFactory.Create<MoveCardsGameAction>().Run(card2, investigator1.DangerZone));


            yield return _gameActionFactory.Create(new MoveCardsGameAction(investigator1.Cards.Take(5).ToList(), investigator1.AidZone)).AsCoroutine();
            if (!DEBUG_MODE) WaitToClick(card).AsTask();
            yield return _gameActionFactory.Create(new InteractableGameAction(false)).AsCoroutine();

            Assert.That(investigator1.DangerZone.TopCard, Is.EqualTo(card));
        }

        private IEnumerator WaitToClick(Card card)
        {
            CardSensorController cardSensor = _cardViewsManager.Get(card).GetComponentInChildren<CardSensorController>();
            yield return new WaitUntil(() => cardSensor.IsClickable);
            cardSensor.OnMouseUpAsButton();
            yield return new WaitUntil(() => cardSensor.IsClickable && _zoneViewManager.SelectorZone.GetComponentsInChildren<CardView>().Length > 0);
            cardSensor.OnMouseUpAsButton();
        }
    }
}
