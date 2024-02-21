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
    public class AvatarTest : TestBase
    {
        [Inject] private readonly PrepareGameUseCase _prepareGameUse;
        [Inject] private readonly GameActionFactory _gameActionFactory;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;
        [Inject] private readonly InvestigatorLoaderUseCase _investigatorLoaderUseCase;
        [Inject] private readonly DataSaveLoaderUseCase _dataSaveLoaderUseCase;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Load_Avatar()
        {
            _dataSaveLoaderUseCase.Execute();

            _investigatorLoaderUseCase.Execute();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_avatarViewsManager.AllAvatars.Count, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator Show_Turns()
        {
            _prepareGameUse.Execute();

            yield return _gameActionFactory.Create(new UpdateStatGameAction(_investigatorsProvider.Leader.InvestigatorCard.Turns, 3)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.Leader).GetPrivateMember<TurnController>("_turnController").ActiveTurnsCount, Is.EqualTo(3));
        }
    }
}