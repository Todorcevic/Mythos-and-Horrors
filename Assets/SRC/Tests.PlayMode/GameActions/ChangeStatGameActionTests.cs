using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ChangeStatGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionProvider _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly ChaptersProvider _chaptersProvider;
        [Inject] private readonly CardViewsManager _cardViewsManager;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Update_Investigator_Stats()
        {
            _prepareGameUse.Execute();

            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.InvestigatorCard, _investigatorsProvider.Leader.InvestigatorZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Health, 3)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, 2)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Leader.Health.Value, Is.EqualTo(3));
            Assert.That((_cardViewsManager.GetCardView(_investigatorsProvider.Leader.AvatarCard) as AvatarCardView).GetPrivateMember<StatView>("_health").Stat.Value, Is.EqualTo(3));
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.Leader).GetPrivateMember<StatUIView>("_healthStat").Stat.Value, Is.EqualTo(3));
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.Leader).GetPrivateMember<TurnController>("_turnController").ActiveTurnsCount, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Move_Resource_From_Card()
        {
            _prepareGameUse.Execute();
            CardSupply cardSupply = _investigatorsProvider.Leader.Cards[0] as CardSupply;

            yield return _gameActionFactory.Create(new MoveCardsGameAction(cardSupply, _chaptersProvider.CurrentScene.PlotZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.ResourceCost, 8)).AsCoroutine();
            while (DEBUG_MODE)
            {
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.ResourceCost, 1)).AsCoroutine();
                yield return PressAnyKey();
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.ResourceCost, 8)).AsCoroutine();
                yield return PressAnyKey();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardSupply.ResourceCost.Value, Is.EqualTo(8));
            Assert.That((_cardViewsManager.GetCardView(cardSupply) as DeckCardView).GetPrivateMember<StatView>("_cost").Stat.Value, Is.EqualTo(8));
        }
    }
}
