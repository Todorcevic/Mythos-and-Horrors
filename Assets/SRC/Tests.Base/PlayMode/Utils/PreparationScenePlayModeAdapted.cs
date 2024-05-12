using MythosAndHorrors.GameRules;
using System.Collections;
using UnityEngine;
using MythosAndHorrors.EditMode.Tests;

namespace MythosAndHorrors.PlayMode.Tests
{
    public abstract class PreparationScenePlayModeAdapted
    {
        protected abstract Preparation Preparation { get; }

        /*******************************************************************/
        public IEnumerator PlaceAllScene()
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return Preparation.PlaceAllScene().AsCoroutine();
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator PlayThisInvestigator(Investigator investigator, bool withCards = true, bool withResources = false, bool withAvatar = true)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return Preparation.PlayThisInvestigator(investigator, withCards, withResources, withAvatar).AsCoroutine();
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator PlayAllInvestigators(bool withCards = true, bool withResources = false, bool withAvatar = true)
        {
            float currentTimeScale = Time.timeScale;
            Time.timeScale = 64;
            yield return Preparation.PlayAllInvestigators(withCards, withResources, withAvatar).AsCoroutine();
            Time.timeScale = currentTimeScale;
        }

        public IEnumerator WasteTurnsInvestigator(Investigator investigator)
        {
            yield return Preparation.WasteTurnsInvestigator(investigator).AsCoroutine();
        }

        public IEnumerator WasteAllTurns()
        {
            yield return Preparation.WasteAllTurns().AsCoroutine();
        }

        public IEnumerator StartingScene(bool withResources = false, bool withAvatar = true)
        {
            yield return PlaceAllScene();
            yield return PlayAllInvestigators(withResources: withResources, withAvatar: withAvatar);
        }
    }
}
