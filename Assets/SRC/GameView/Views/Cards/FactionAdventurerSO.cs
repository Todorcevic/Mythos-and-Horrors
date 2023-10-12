using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/AdventurerElements")]
    public class FactionAdventurerSO : ScriptableObject
    {
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateFront;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _badget;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _health;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _sanity;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _stats;
    }
}
