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
    public class AvatarTest : TestBase
    {
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly AvatarViewsManager _avatarViewsManager;

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator Load_Avatar()
        {
            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_avatarViewsManager.AllAvatars.Count, Is.EqualTo(4));
        }

        [UnityTest]
        public IEnumerator Show_Turns()
        {
            yield return _gameActionsProvider.Create(new UpdateStatGameAction(_investigatorsProvider.First.CurrentTurns, 3)).AsCoroutine();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(_avatarViewsManager.Get(_investigatorsProvider.First).GetPrivateMember<TurnController>("_turnController").ActiveTurnsCount, Is.EqualTo(3));
        }
    }
}