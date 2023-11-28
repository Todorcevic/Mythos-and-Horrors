using DG.Tweening;
using MythsAndHorrors.GameRules;
using MythsAndHorrors.GameView;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using Zenject;

namespace MythsAndHorrors.PlayMode.Tests
{
    [TestFixture]
    public class SwapAdventurerTest : TestBase
    {
        private readonly bool DEBUG_MODE = true;
        [Inject] private readonly AdventurersProvider _adventurersProvider;
        [Inject] private readonly ZoneViewsManager _zonesManager;
        [Inject] private readonly SwapAdventurerComponent _swapAdventurerComponent;
        [Inject] private readonly CardViewGeneratorComponent _cardGenerator;
        [Inject] private readonly PrepareGameUseCase _prepareGameUseCase;

        /*******************************************************************/
        [UnitySetUp]
        public override IEnumerator SetUp()
        {
            yield return base.SetUp();
        }

        /*******************************************************************/

        [UnityTest]
        public IEnumerator Prepare_Full_Adventurer()
        {
            SaveData saveData = new()
            {
                AdventurersSelected = new List<string>() { "01501", "01502" },
                SceneSelected = "COREScene1"
            };

            _prepareGameUseCase.Execute(saveData);
            Adventurer adventurer1 = _adventurersProvider.AllAdventurers[0];
            Adventurer adventurer2 = _adventurersProvider.AllAdventurers[1];

            ZoneView adventurer1Zone = _zonesManager.Get(adventurer1.HandZone);
            CardView oneCard = _cardGenerator.BuildCard(adventurer1.AdventurerCard);
            yield return adventurer1Zone.EnterCard(oneCard).WaitForCompletion();

            ZoneView adventurer2Zone = _zonesManager.Get(adventurer2.HandZone);
            CardView twoCard = _cardGenerator.BuildCard(adventurer2.AdventurerCard);
            yield return adventurer2Zone.EnterCard(twoCard).WaitForCompletion();

            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer2).WaitForCompletion();
            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer1).WaitForCompletion();
            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer2).WaitForCompletion();
            yield return PressAnyKey();
            yield return _swapAdventurerComponent.Select(adventurer1).WaitForCompletion();
            yield return PressAnyKey();

            if (DEBUG_MODE) yield return new WaitForSeconds(230);
        }


    }
}
