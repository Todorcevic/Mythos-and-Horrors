using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using System.Collections;
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

        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator UndoMoveMulticardsTest()
        {
            yield return PlayThisInvestigator(_investigatorsProvider.First);
        }
    }
}
