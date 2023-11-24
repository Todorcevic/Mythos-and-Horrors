using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace MythsAndHorrors.GameView
{
    public class AdventurerAreaComponent : MonoBehaviour
    {
        [Inject] private readonly DiContainer _diContainer;
        [SerializeField, Required, AssetsOnly] private AreaAdventurerView _adventurerAreaPrefab;

        /*******************************************************************/
        public void BuildAdventurerArea(Adventurer adventurer) =>
          _diContainer.InstantiatePrefabForComponent<AreaAdventurerView>(_adventurerAreaPrefab, transform, new object[] { adventurer });
    }
}
