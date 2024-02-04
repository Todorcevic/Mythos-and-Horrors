using MythsAndHorrors.GameRules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace MythsAndHorrors.GameView
{
    [CreateAssetMenu(fileName = "Faction", menuName = "ScriptableObjects/InvestigatorElements")]
    public class FactionInvestigatorSO : ScriptableObject
    {
        [SerializeField, Required] public Faction _faction;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templateFront;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _templatePlayCard;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _badget;
        [SerializeField, Required, ChildGameObjectsOnly] public Sprite _stats;
        [SerializeField, Required] public Color _color;
    }
}
