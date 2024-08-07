using MythosAndHorrors.GameRules;
using UnityEngine.TestTools;
using System.Collections;
using NUnit.Framework;
using System.Threading.Tasks;
using MythosAndHorrors.PlayMode.Tests;

namespace MythosAndHorrors.PlayModeCORE1.Tests
{
    public class GameActionOpportunityAttackTests : TestCORE1Preparation
    {
        //protected override TestsType TestsType => TestsType.Debug;

        [UnityTest]
        public IEnumerator OpportunityAttackWhenInvestigate()
        {
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            Investigator investigator = _investigatorsProvider.First;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);

            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(place, _chaptersProvider.CurrentScene.GetPlaceZone(2, 2)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, place).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(place);
            yield return ClickedMainButton();
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator OpportunityAttackAnyCreatureWithSafeForeachAndEliminate()
        {
            CardPlace place = _cardsProvider.GetCard<Card01111>();
            Investigator investigator = _investigatorsProvider.Fourth;
            CardCreature creature = SceneCORE1.GhoulSecuaz;
            CardCreature creature2 = SceneCORE1.GhoulVoraz;
            _ = MustBeRevealedThisToken(ChallengeTokenType.Value1);

            yield return PlayThisInvestigator(investigator);
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(place, _chaptersProvider.CurrentScene.GetPlaceZone(2, 2)).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveInvestigatorToPlaceGameAction>().SetWith(investigator, place).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<MoveCardsGameAction>().SetWith(creature2, investigator.DangerZone).Execute().AsCoroutine();
            yield return _gameActionsProvider.Create<HarmToCardGameAction>().SetWith(creature2, investigator.InvestigatorCard, amountDamage: 2).Execute().AsCoroutine();

            Task gameActionTask = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(investigator).Execute();
            yield return ClickedIn(place);
            yield return ClickedIn(investigator.InvestigatorCard);
            yield return ClickedIn(creature2);
            yield return ClickedMainButton();
            yield return ClickedMainButton();

            yield return gameActionTask.AsCoroutine();

            Assert.That(investigator.DamageRecived.Value, Is.EqualTo(1));
            Assert.That(investigator.FearRecived.Value, Is.EqualTo(1));
        }
    }
}
