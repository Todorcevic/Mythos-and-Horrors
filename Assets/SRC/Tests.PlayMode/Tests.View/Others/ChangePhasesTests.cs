using DG.Tweening;
using MythosAndHorrors.GameRules;
using MythosAndHorrors.GameView;
using MythosAndHorrors.PlayMode.Tests;
using NUnit.Framework;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythosAndHorrors.PlayModeView.Tests
{
    [TestFixture]
    public class ChangePhasesTests : PlayModeTestsBase
    {
        [Inject] protected readonly PhaseComponent _phaseComponent;
        //protected override bool DEBUG_MODE => true;

        /*******************************************************************/
        [UnityTest]
        public IEnumerator ChangePhases()
        {
            PlayInvestigatorGameAction investigatorPhaseGameAction = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.Third);
            PlayInvestigatorGameAction investigatorPhaseGameAction2 = _gameActionsProvider.Create<PlayInvestigatorGameAction>().SetWith(_investigatorsProvider.First);
            CreaturePhaseGameAction creaturePhaseGameAction = _gameActionsProvider.Create<CreaturePhaseGameAction>();

            if (DEBUG_MODE) Time.timeScale = 1;
            while (DEBUG_MODE)
            {
                yield return PressAnyKey();
                yield return _phaseComponent.ShowThisPhase(investigatorPhaseGameAction).WaitForCompletion();
                yield return PressAnyKey();
                yield return _phaseComponent.ShowThisPhase(creaturePhaseGameAction).WaitForCompletion();
                yield return PressAnyKey();
                yield return _phaseComponent.ShowThisPhase(investigatorPhaseGameAction2).WaitForCompletion();
            }

            yield return _phaseComponent.ShowThisPhase(investigatorPhaseGameAction).WaitForCompletion();
            Assert.That(_phaseComponent.GetPrivateMember<PhaseView>("_currentPhaseView").Phase, Is.EqualTo(Phase.Investigator));
        }
    }
}
