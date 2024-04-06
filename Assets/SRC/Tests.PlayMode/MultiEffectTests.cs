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
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly ZoneViewsManager _zoneViewManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator MultiEffect_Test()
        {
            InteractableGameAction interactableGameAction = new(isUndable: true);
            Investigator investigator1 = _investigatorsProvider.First;
            Card card = investigator1.Cards[1];
            Card card2 = investigator1.Cards[2];

            interactableGameAction.CreateMainButton()
                     .SetDescription("Continue")
                     .SetLogic(() => Task.CompletedTask);

            Effect effectToClick = interactableGameAction.Create()
                   .SetCard(card)
                   .SetInvestigator(investigator1)
                   .SetDescription("EffectOne")
                   .SetLogic(() => _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator1.DangerZone)));

            interactableGameAction.Create()
                .SetCard(card)
                .SetInvestigator(investigator1)
                .SetDescription("EffectTwo")
                .SetLogic(() => _gameActionsProvider.Create(new MoveCardsGameAction(card, investigator1.HandZone)));

            interactableGameAction.Create()
                .SetCard(card2)
                .SetInvestigator(investigator1)
                .SetDescription("EffectOne")
                .SetLogic(() => _gameActionsProvider.Create(new MoveCardsGameAction(card2, investigator1.DangerZone)));

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(investigator1.Cards.Take(5).ToList(), investigator1.HandZone)).AsCoroutine();
            if (!DEBUG_MODE) WaitToClick2(card).AsTask();
            yield return _gameActionsProvider.Create(interactableGameAction).AsCoroutine();

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
