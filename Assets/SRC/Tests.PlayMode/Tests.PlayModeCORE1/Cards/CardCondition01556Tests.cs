﻿using MythosAndHorrors.GameRules;
using NUnit.Framework;
using System.Collections;
using UnityEngine.TestTools;
using MythosAndHorrors.PlayMode.Tests;
using System.Threading.Tasks;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class CardCondition01556Tests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator SwapTokenValue()
        {
            Investigator investigator = _investigatorsProvider.Third;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value_2).ContinueWith((_) => MustBeRevealedThisToken(ChallengeTokenType.Value_2));
            yield return PlaceOnlyScene();
            yield return PlayThisInvestigator(investigator);
            yield return BuilCard("01556", investigator);
            Card01556 conditionCard = _cardsProvider.GetCard<Card01556>();

            yield return _gameActionsProvider.Create(new MoveCardsGameAction(conditionCard, investigator.HandZone)).AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create(new PlayInvestigatorGameAction(investigator));
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedIn(conditionCard);
            yield return ClickedIn(investigator.CurrentPlace);
            yield return ClickedMainButton();
            yield return ClickedMainButton();
            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.Hints.Value, Is.EqualTo(1));
        }
    }
}