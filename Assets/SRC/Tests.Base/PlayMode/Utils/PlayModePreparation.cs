using MythosAndHorrors.EditMode.Tests;
using MythosAndHorrors.GameRules;
using System.Collections;
using UnityEngine;
using Zenject;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class PlayModePreparation
    {
        [Inject] private readonly PreparationSceneCORE1 _preparation;

        /*******************************************************************/
        public abstract IEnumerator PlaceAllScene();

        public IEnumerator PlayThisInvestigator(Investigator investigator, bool withCards = true, bool withResources = false, bool withAvatar = true)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return _preparation.PlayThisInvestigator(investigator, withCards, withResources, withAvatar).AsCoroutine();
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator PlayAllInvestigators(bool withCards = true, bool withResources = false, bool withAvatar = true)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return _preparation.PlayAllInvestigators(withCards, withResources, withAvatar).AsCoroutine();
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator WasteTurnsInvestigator(Investigator investigator)
        {
            yield return _preparation.WasteTurnsInvestigator(investigator).AsCoroutine();
        }

        public IEnumerator WasteAllTurns()
        {
            yield return _preparation.WasteAllTurns().AsCoroutine();
        }

        public IEnumerator StartingScene(bool withResources = false, bool withAvatar = true)
        {
            yield return PlaceAllScene();
            yield return PlayAllInvestigators(withResources: withResources, withAvatar: withAvatar);
        }
    }
}
