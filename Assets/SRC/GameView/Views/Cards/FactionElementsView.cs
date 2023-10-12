using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/FactionElements")]
    public class FactionElementsView : ScriptableObject
    {
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateFront;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _badget;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _health;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _stats;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateDeckFront;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _cost;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _assistant;
    }
}
