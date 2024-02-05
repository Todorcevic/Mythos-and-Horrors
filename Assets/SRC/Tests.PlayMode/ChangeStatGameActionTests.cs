using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class ChangeStatGameActionTests : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionFactory _gameActionFactory;
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
            CardPlace placeCard = _chaptersProvider.CurrentScene.Info.PlaceCards[0];

            yield return _gameActionFactory.Create(new MoveCardsGameAction(_investigatorsProvider.Leader.InvestigatorCard, _investigatorsProvider.Leader.InvestigatorZone)).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveCardsGameAction(placeCard, _chaptersProvider.CurrentScene.PlaceZone[2, 2])).AsCoroutine();
            yield return _gameActionFactory.Create(new MoveInvestigatorGameAction(_investigatorsProvider.Leader, placeCard)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.InvestigatorCard.Health, 3)).AsCoroutine();
            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.Turns, 2)).AsCoroutine();


            if (DEBUG_MODE) yield return new WaitForSeconds(230);

            Assert.That(_investigatorsProvider.Leader.InvestigatorCard.Health.Value, Is.EqualTo(3));
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
            yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.Cost, 8)).AsCoroutine();
            while (DEBUG_MODE)
            {
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.Cost, 1)).AsCoroutine();
                yield return PressAnyKey();
                yield return _gameActionFactory.Create(new UpdateStatGameAction(cardSupply.Cost, 8)).AsCoroutine();
                yield return PressAnyKey();
            }

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(cardSupply.Cost.Value, Is.EqualTo(8));
            Assert.That((_cardViewsManager.GetCardView(cardSupply) as DeckCardView).GetPrivateMember<StatView>("_cost").Stat.Value, Is.EqualTo(8));
        }
    }
}
