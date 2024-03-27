using ModestTree;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public class ChallengeTests : TestBase
    {
        [Inject] private readonly ChallengeBagComponent _challengeBagComponent;
        [Inject] private readonly InvestigatorsProvider _investigatorsProvider;
        [Inject] private readonly GameActionsProvider _gameActionsProvider;
        [Inject] private readonly UndoGameActionButton _undoGameActionButton;

        protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator PushTokenTest()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);

            do
            {
                if (DEBUG_MODE) yield return PressAnyKey();
                yield return _challengeBagComponent.DropToken(new ChallengeToken(ChallengeTokenType.Danger)).AsCoroutine();
            } while (DEBUG_MODE);

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
            Assert.That(true);
        }
    }
}
