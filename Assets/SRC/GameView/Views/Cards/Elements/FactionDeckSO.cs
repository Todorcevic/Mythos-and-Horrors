using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/DeckElements")]
    public class FactionDeckSO : ScriptableObject
    {
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateDeckFront;
        [SerializeField, ChildGameObjectsOnly] public Sprite _badget;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _titleHolder;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _skillHolder;
    }
}
