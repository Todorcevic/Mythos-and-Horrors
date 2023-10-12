using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/DeckElements")]
    public class FactionDeckSO : ScriptableObject
    {
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateDeckFront;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _badget;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _cost;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _assistant;
    }
}
